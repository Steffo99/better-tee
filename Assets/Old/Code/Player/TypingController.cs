using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BetterTee.Player {
        
    public class TypingController : ActController
    {
        public List<String> submissions;

        [Header("Prefabs")]
        public GameObject actNamePrefab;
        public GameObject actDescriptionPrefab;
        public GameObject timerPrefab;
        public GameObject submissionFieldPrefab;
        public GameObject submitPrefab;
        public GameObject submittedCountPrefab;

        [Header("Objects")]
        protected Text actName;
        protected Text actDescription;
        protected Timer actTimer;
        protected InputField submissionField;
        protected Submit submit;
        protected Text submittedCount;

        protected override void Start() {
            base.Start();
            submissions = new List<string>();
        }

        public override void ActInit() {
            base.ActInit();

            //Load settings
            TypingSettings typingSettings = settings as TypingSettings;

            //Init actName Text
            actName = Instantiate(actNamePrefab, canvas.transform).GetComponent<Text>();
            actName.text = typingSettings.actName;

            //Init actDescription Text
            actDescription = Instantiate(actDescriptionPrefab, canvas.transform).GetComponent<Text>();
            actDescription.text = typingSettings.actDescription;

            //Init actTimer
            actTimer = Instantiate(timerPrefab, canvas.transform).GetComponent<Timer>();
            actTimer.TimerSet(typingSettings.timeLimit);

            //Init submissionInputField
            submissionField = Instantiate(submissionFieldPrefab, canvas.transform).GetComponent<InputField>();
            
            //Init submit Button
            submit = Instantiate(submitPrefab, canvas.transform).GetComponent<Submit>();
            submit.typingController = this;
            submit.inputField = submissionField;

            //Init submissionCount Text
            submittedCount = Instantiate(submittedCountPrefab, canvas.transform).GetComponent<Text>();
            submittedCount.text = "";
        }

        public override void ActStart() {
            base.ActStart();

            //Enable submissionField and submit Button
            submissionField.interactable = true;
            submit.GetComponent<Button>().interactable = true;

            //Focus on submissionField
            eventSystem.SetSelectedGameObject(submissionField.gameObject);
            
            //Start timer
            actTimer.OnTimeOut += new Action(ActEnd);
            actTimer.TimerStart();
        }

        public override void ActEnd() {
            base.ActEnd();

            //Disable submissionField and submit Button
            submissionField.interactable = false;
            submit.GetComponent<Button>().interactable = false;

            //Create results
            results = new TypingResults {
                submissions = submissions.ToArray()
            };
        }

        public override void ActCleanup() {
            base.ActCleanup();

            Destroy(actName.gameObject);
            Destroy(actDescription.gameObject);
            Destroy(actTimer.gameObject);
            Destroy(submissionField.gameObject);
            Destroy(submit.gameObject);
            Destroy(submittedCount.gameObject);
        }

        public void Submit(string submission) {
            if(submission != "") {
                submissions.Add(submission);
                submittedCount.text = String.Format("Submitted: {0}", submissions.Count);
            }
        }
    }

}