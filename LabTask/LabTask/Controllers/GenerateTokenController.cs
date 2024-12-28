using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LabTask.Controllers
{
    public class GenerateTokenController : Controller
    {
        private const string TokenSessionKey = "Tokens";
        private const string CalledTokensSessionKey = "CalledTokens";
        private const string CounterSessionKey = "GlobalCounter";
        private const string DateSessionKey = "TokenDate";

        private const int MaxGlobalTokens = 100;
        private const int MaxPerTypeTokens = 25;

        public ActionResult Token()
        {
            ResetIfNewDay();

            if (Session[TokenSessionKey] == null) Session[TokenSessionKey] = new List<string>();
            if (Session[CalledTokensSessionKey] == null) Session[CalledTokensSessionKey] = new Dictionary<string, string>();

            ViewBag.Tokens = Session[TokenSessionKey] as List<string>;
            SetRemainingTokensInfo();

            return View();
        }

        [HttpPost]
        public ActionResult Token(string visaType)
        {
            ResetIfNewDay();

            if (string.IsNullOrEmpty(visaType))
            {
                ViewBag.Message = "Please select a visa type.";
                SetRemainingTokensInfo();
                return View();
            }

            List<string> tokens = Session[TokenSessionKey] as List<string>;
            int globalCounter = Session[CounterSessionKey] == null ? 1 : (int)Session[CounterSessionKey];

            if (tokens.Count >= MaxGlobalTokens)
            {
                ViewBag.Message = "Maximum limit of 100 tokens reached.";
                SetRemainingTokensInfo();
                return View();
            }

            int typeCount = tokens.Count(t => t.StartsWith(visaType));
            if (typeCount >= MaxPerTypeTokens)
            {
                ViewBag.Message = $"Maximum limit of 25 tokens for {visaType} reached.";
                SetRemainingTokensInfo();
                return View();
            }

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string newToken = $"{visaType} {globalCounter} - {timestamp}";
            tokens.Add(newToken);

            Session[TokenSessionKey] = tokens;
            Session[CounterSessionKey] = globalCounter + 1;

            ViewBag.NewToken = newToken;
            ViewBag.Tokens = tokens;

            SetRemainingTokensInfo();
            return View();
        }

        public ActionResult CallCustomer(string counter)
        {
            ResetIfNewDay();

            if (Session[TokenSessionKey] == null) Session[TokenSessionKey] = new List<string>();
            if (Session[CalledTokensSessionKey] == null) Session[CalledTokensSessionKey] = new Dictionary<string, string>();

            List<string> tokens = Session[TokenSessionKey] as List<string>;
            Dictionary<string, string> calledTokens = Session[CalledTokensSessionKey] as Dictionary<string, string>;

            if (string.IsNullOrEmpty(counter))
            {
                ViewBag.Message = "Please select a valid counter.";
                return View();
            }

            // Find the first available token for the selected counter
            var nextToken = tokens.FirstOrDefault(t => t.StartsWith(counter));

            if (nextToken != null)
            {
                // Update the called token list and remove from the main token list
                calledTokens[counter] = nextToken;
                tokens.Remove(nextToken);

                Session[TokenSessionKey] = tokens;
                Session[CalledTokensSessionKey] = calledTokens;

                ViewBag.CalledToken = nextToken;
            }
            else
            {
                ViewBag.Message = $"No tokens left for counter: {counter}.";
            }

            ViewBag.CalledTokens = calledTokens;
            return View();
        }

        private void SetRemainingTokensInfo()
        {
            List<string> tokens = Session[TokenSessionKey] as List<string>;

            int globalRemaining = MaxGlobalTokens - tokens.Count;
            int medRemaining = MaxPerTypeTokens - tokens.Count(t => t.StartsWith("Med"));
            int trRemaining = MaxPerTypeTokens - tokens.Count(t => t.StartsWith("Tr"));
            int bRemaining = MaxPerTypeTokens - tokens.Count(t => t.StartsWith("B"));
            int goRemaining = MaxPerTypeTokens - tokens.Count(t => t.StartsWith("GO"));

            ViewBag.GlobalRemaining = globalRemaining;
            ViewBag.MedRemaining = medRemaining;
            ViewBag.TrRemaining = trRemaining;
            ViewBag.BRemaining = bRemaining;
            ViewBag.GoRemaining = goRemaining;
        }

        private void ResetIfNewDay()
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            if (Session[DateSessionKey] == null || Session[DateSessionKey].ToString() != currentDate)
            {
                Session[TokenSessionKey] = new List<string>();
                Session[CalledTokensSessionKey] = new Dictionary<string, string>();
                Session[CounterSessionKey] = 1;
                Session[DateSessionKey] = currentDate;
            }
        }
    }
}
