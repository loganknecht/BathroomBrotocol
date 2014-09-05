using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextLibrary {
	public Dictionary<string, string> gameText = new Dictionary<string, string>();

	private static volatile TextLibrary _instance;
	private static object _lock = new object();
	
	//Stops the lock being created ahead of time if it's not necessary
	static TextLibrary() {
	}
	
	public static TextLibrary Instance {
		get {
			if (_instance == null) {
				lock(_lock) {
					if (_instance == null) 
						_instance = new TextLibrary();
				}
			}
			return _instance;
		}
	}
	
	private TextLibrary() {
		LoadGameTextFromCode();
	}

	void LoadGameTextFromCode() {
		LoadBasicTutorialText();
	}

	void LoadBasicTutorialText() {
		//Basic Tutorial Text
		gameText.Add("BasicTutorialNotStarted", "Basic Tutorial Not Started.");
		gameText.Add("BasicTutorialParagraphOne", "SO! I see you're one of the new bro czars in this business! Well... I appreciate that, but right now you're just cleaning up poop my brother. So let's get one thing straight: Right now you're just a chump, and no one respects you! You gotta earn it!");
		gameText.Add("BasicTutorialParagraphTwo", "With that said, welcome to my daily world of brotocol. You see, our jobs are one of the most important roles in his bro-ness' cabinet! I mean... they laugh at us sure, but we run shit! Well... you know metaphorically and professionally...");
		gameText.Add("BasicTutorialParagraphThree", "Anyway! Where was I? Oh! Yes, sorry, sorry. Welcome, welcome to... one of the better bathrooms in Broledo, Brohio.\n\n Look, I know... It's not the best assignment.. But, think of it as an opportunity to practice your fundamentals. And! if you're good I'll give you some tipes about brotocol. But don't tell anyone I told you anything!\n\nI don't want to be disbronorably discharged like that jerk Chad.");
		gameText.Add("BasicTutorialParagraphFour", "Ahem. What you're seeing right now is a rare sight: an empty bathroom. In this bathroom you have your bare essentials: an entrance, three urinals, and an exit. I'm going to cover them in order.");
		gameText.Add("BasicTutorialParagraphFive", "First is your entrance queue. This is where all bros come to greet you. They will wait here, waiting and demanding a spot to be directed to so they can relieve themselves\n\nIf a bro is in this location it's your job to select the bro and point him to a location to relieve himself, by selecting its.");
		gameText.Add("BasicTutorialParagraphSix", "These! These are urinals. Hah, not your-inals, not my-inals either. Urinals. The point is that everyone should know how these work. Bros come in and they urinate here to relieve themselves.");
		gameText.Add("BasicTutorialParagraphSeven", "After the bros have urinated they will do a couple of things depending on the bathroom setup.\n\nIf there is a sink queue, and sinks are in the bathroom, a bro will wait in the sink queue so that you can tell him where he should wash his hands.\nIf there is no sink queue, but there are sinks, the bros will go to one of the sinks in a random order. You will have to watch them so they don't go to the sinks at the same time.\nIf there is no sink queue and there are no sinks, then the bros will find the nearest exit and leave.");
		gameText.Add("BasicTutorialParagraphEight", "Speaking of exits, here is the exit to this restroom. Once a bro has used the restroom, and washed their hands they can they leave.");
		gameText.Add("BasicTutorialParagraphNine", "Phew.\n\nWith all that said I’m going to go stand over here, and let a couple of bros in that we can practice directing with.");
		gameText.Add("BasicTutorialParagraphTen", "Alright, I’m about to let in two bros. You need to send each one of these to a urinal for relief.");
		gameText.Add("BasicTutorialParagraphEleven", "Here...");
		gameText.Add("BasicTutorialParagraphTwelve", "We...");
		gameText.Add("BasicTutorialParagraphThirteen", "Go!");

		gameText.Add("BasicTutorialPracticeOneStart", "");

		gameText.Add("BasicTutorialPracticeOneFirstBroServed", "Nice job! That's bro one down! Here comes bro number two!");
		gameText.Add("BasicTutorialPracticeOneSecondBroServed", "Wow. You are pretty good at this! I can see why they chose you as the next bathroom bro czar.");

		gameText.Add("BasicTutorialParagraphFourteen", "Alright then. I'm not really supposed to condone this sort of brohavior, but I need to show you something so you're prepared for when we're doing some serious work.");
		gameText.Add("BasicTutorialParagraphFifteen", "Some bros have to do more than relieve their bladders. Some bros need to poop. However if a bro is sent to a urinal when they need to poop... well it's just better to show you.");
		gameText.Add("BasicTutorialParagraphSixteen", "Go ahead and help this bro relieve himself.");

		gameText.Add("BasicTutorialPracticeOnePoopingBroStart", "");
		gameText.Add("BasicTutorialPracticeOnePoopingBroServed", "And, this is why we can't have nice things! Again, I don't condone this sort of behaviour, and neither should you! Be aware that when this occurs it will reflect badly on your evaluation.");

		gameText.Add("BasicTutorialParagraphSeventeen", "I will touch more on your evaulation in a moment, but for now let's get this little accident cleaned up...");
		gameText.Add("BasicTutorialParagraphEighteen", "In order to clean this up all you need to do is tell me what to clean up. I am your personal janitor after all.\n\nIn order to do this just select me, then select the accident that was made. I'll handle the rest.");

		gameText.Add("BasicTutorialPracticeOneJanitorCleaningStarted", "");

		gameText.Add("BasicTutorialParagraphNineteen", "And that's basically it!");
		gameText.Add("BasicTutorialParagraphTwenty", "Oh! And one one more thing, before I forget!\n\nPlease, please, please try to follow bathroom brotocol. His broness looks favorably on us so long as we adhere to bathroom brotocol.");
		gameText.Add("BasicTutorialParagraphTwentyOne", "What is bathroom brotocol...?\n\nUm... Well... We can't really talk about that openly...");
		gameText.Add("BasicTutorialParagraphTwentyTwo", "What I can do is tell you that there are rules that each patron of a bathroom needs to follow when using the facilities, and if they don't... well then they're not following bathroom brotocol.");
		gameText.Add("BasicTutorialParagraphTwentyThree", "Your rating from his broness is determined by your abilitiy to serve patrons, and adhere to bathroom brotocol. Make sure to keep an eye out on our patrons so that they can be served to their fullest.");

		gameText.Add("BasicTutorialEnd", "Alright, that really is all of it! Good luck! I know you'll do amazing!");
	}
}
