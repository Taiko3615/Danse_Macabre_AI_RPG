using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        [SerializeField] private InputField inputField;
        [SerializeField] private Button button;

        //Locations
        [SerializeField] private Button gardenButton;
        private bool gardenVisited = false;
        [SerializeField] private Button scriptoriumButton;
        private bool scriptoriumVisited = false;
        [SerializeField] private Button mortuaryButton;
        private bool mortuaryVisited = false;
        [SerializeField] private Button archivesButton;
        private bool archivesVisited = false;
        [SerializeField] private Button cryptButton;
        private bool cryptVisited = false;
        [SerializeField] private RectTransform context;

        //Clues
        bool clueMissingHeart = false;

        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;

        private NPC currentNPC;

        private float height;

        private void Start()
        {
            button.onClick.AddListener(SendReply);
            gardenButton.onClick.AddListener(Garden);
            scriptoriumButton.onClick.AddListener(Scriptorium);
            mortuaryButton.onClick.AddListener(Mortuary);
            archivesButton.onClick.AddListener(Archives);
            cryptButton.onClick.AddListener(Crypt);
            Garden();

        }

        private async void AppendMessage(ChatMessage message)
        {
            context.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, context);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            item.anchoredPosition = new Vector2(0, -height);

            await Task.Delay(200);

            height += item.sizeDelta.y;
            context.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

        private async void SendReply()
        {
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = inputField.text
            };
            
            AppendMessage(newMessage);

            button.enabled = false;
            inputField.enabled = false;
            
            // Complete the instruction
            var response = await currentNPC.Say(inputField.text);

            AppendMessage(new ChatMessage()
            {
                Role = "assistant",
                Content = response
            });

            button.enabled = true;
            inputField.enabled = true;
        }

        private static string globalContext = @"You are a Game Master in a medieval realistic game.
	    You will talk to me as you would to a player of this game and your answers should always be one paragraph long and descriptive in a very educated writing style.
	    We are in 1344 in Austria.
	    The player is investigating the death of Friedrich II von Habsburg.

	    If the players do anything very illogical, explain that they can’t do that.
	    If the players do anything or say anything that isn’t possible in the middle age, explain that they can’t do that.
	    If the player does anything related to magic, he can’t do that.
	    If the players start to do things unrelated to the investigation of the death of Friedrich II von Habsburg, explain that they have to focus on this investigation.
	    If the players want to leave the abbey, explain that they have to focus on this investigation.
	    Never give the players a list of actions that they can perform.

	    More Context :
	    Friedrich II von Habsburg died in the Neuberg Abbey the 11 December 1344 at the age of 17. His death is very suspicious because he was the last member of this branch of the House of Habsburg. The Pope wants the inquisition (the players) to investigate, because it is the third member of this family to die within few years in this abbey. Otto der Fröhliche in 1339 then Leopold II von Habsburg in August 1344 and now Friedrich II in December. Their rival Rudolf IV made it very clear to the pope, that he doesn't want any noise. The pope can investigate, but no conclusions can be made public. Officially, they died of illness and that's it.

	    Both Otto der Fröhliche and Leopold II von Habsburg are burried in the Abbey's crypt.

	    The Societa Templois was a holy military order created by Otto der Fröhliche to fight pagans in the east. Otto also was the main mecene of this Abbey, so their archives are kept here in the basement.

	    Today is the 20th of December 1344, Friedrich II died 9 days ago.

	    Friedrich II von Habsburg was very secluded young man, he would rarely speak and had few friends. But everyone liked him at the abbey. The last person to see him alive was Brother Eudes, the cook who is currently in the kitchen. He saw him at breakfast but he was silent.
	    The corpse was found in the garden at 6am by the Abbot.

	    He only had one real friend : Brother Conrad, a scribe who is currently in the scriptorium. He is the last person to talk to him when he was alive.

	    There are 5 important locations, remember them : The 3 floors of the library, the mortuary, the archives of the Societa Templois in the basement, the crypt where Otto der Fröhliche and Leopold II von Habsburg are burried.

	    There are 2 important characters, remember them : Reverend Father Albrecht the Abbot who is currently in the garden, Brother Galeazzo the doctor who is currently in the mortuary.";


        private void Garden()
        {
            currentNPC = new NPC(globalContext, @"The player is currently talking to Reverend Father Albrecht in the garden, answer like this character would.

			Reverend Father Albrecht is the Abbot of the abbey, he didn't know Friedrich II very well, but he appreciated the young man.
			He is the one who found the corpse at 6am in the garden.

			Reverend Father Albrecht enjoys gardening, it is his favourite activity.
			He is quite old and he seems lost in his thoughts.

			In particular he is very very worried about will happen to the abbey now that their most important donors are dead. Almost of the funding of the abbey came from their generous patronage.

			Ambiance :

			Reverend Father Albrecht is old and lost in thoughts. He is always carrying a rosary and it seems that he is reciting prayers while talking to the players.

			Actions :

			If the player asks if Reverend Father Albrecht why he called the pope for help
			He will reply that it is the third death in the abbey and he would really need some help to understand what happened.
			He belives that the Inquisition will know how to be discreet about the investigation, which is in everyone's interest.

			If the player asks if Reverend Father Albrecht saw anything unual at the abbey
			He will start by trying to avoid the subjet politely. Then if the player insists.
			He will reluctantly say that things became very messy recently, the scribes are doing a worse and worse job, sometimes making obvious mistakes or being very negligent in their work.
			Some monks are missing Mass, which is a grave offence.
			And some monks are seen wandering in the alleys after curfew.

			If the players ask why there are so many unual things in the abbey
			He will say that he doesn't know, everyone seems so busy and burnout.

			If the players ask about the corpse of Friedrich II and if anything unusual on his body
			He will say that he doesn't know and encourage the players to go to the mortuaty to check it.

			Remember : You will talk to me as you would to a player of this game and your answers should always be one paragraph long and descriptive in a very educated writing style.",
            @"You walk towards Reverend Father Albrecht.
			The Abbot is a tall, stooped man with a kindly face and deep-set eyes. He moves slowly, his steps measured and deliberate, as if he is lost in thought.

			His long white beard falls in waves over his chest, and his hands are gnarled and work-worn. Despite his advanced age, Reverend Father Albrecht is still a commanding figure, and his presence exudes a sense of authority and wisdom. He is often seen in the garden, his favorite place, where he spends his time tending to the plants and flowers.

			As he speaks with the players, he holds a rosary in his hand, the beads clicking softly together as he recites his prayers. It is clear that he is a man of great faith, and his devotion to the abbey and its inhabitants is unwavering.

			""Oh, I see that you finally arrived.I am the Abbot Reverend Father Albrecht, delighted to see that His Holiness has agreed to assist me at my request.""");

            if(gardenVisited)
            {
                AppendMessage(new ChatMessage()
                {
                    Role = "assistant",
                    Content = "(You are back in the garden with the Abbot Reverend Father Albrecht.)"
                });
            } else
            {
                    AppendMessage(new ChatMessage()
                {
                    Role = "assistant",
                    Content = currentNPC.lastMessage()
                });
            }

            gardenVisited = true;

        }

        private void Scriptorium()
        {
            currentNPC = new NPC(globalContext,@"The player is currently talking to Brother Conrad in the scriptorium, answer like this character would.


            Brother Conrad was the only real friend of Friedrich II, he was his confessor which is why they liked each other.

            He last saw Friedrich II the day before he died, he was always carrying a book and seemed distrubed.He wanted to confess something very important by Brother Conrad didn't have time.


            Brother Conrad speaks in very weird sentences because is actually completely mad, his face has nervous twitches and he scratches his arm nervously.

            Ambiance :


            Brother Conrad has massive dark circles and nervous tics.He seems obsessed with his work.His eyes are crazy and look in random directions.

            Actions :


            If the player asks to see the what Conrad is currently working on.

            You should describe that is currently writing a copy of the Divine Comedy by Dante Alighieri, but it is very difficult to understand which part of the book he is because his handwriting is barely legible.


            If the player asks to see some other previous work by Conrad.

            Describe that conrad gives them some of his previous work and his writing used to be good, but he gradually started writing less and less good to a point where his writing is now barely legible.


            If the players ask why his writing degraded or why he is so stressed and what's wrong with him.

            Conrad says that he is burnout and work too much and that's why.


            If the player ask about Friedrich II von Habsburg and how he died

            Conrad doesn't know, but he is very sure that it has something to do with the book he was carrying all the time. Is sure that the books are evil and they ate his soul slowly slowly little by little by little.


            If the player ask why he think the books are evil

            Conrad says that he hear them moaning sometimes at night, especially in the archives of the Societa Templois, he hears them like beasts trying to drain the souls of the monks.

            Remember : You will talk to me as you would to a player of this game and your answers should always be one paragraph long and descriptive in a very educated writing style.",
            @"You walk towards Brother Conrad in the scriptorium.
            He is a thin and gaunt figure, with sharp features and deep - set eyes that constantly dart around the room.His face is marked by dark circles and nervous tics, which he tries to suppress by scratching his arm nervously.His hair is unkempt, falling in lanky strands around his face, and his clothes are rumpled and stained with ink.


            Despite his outward appearance, Brother Conrad is focused on his work in the scriptorium.He is currently hunched over a desk, his quill scratching furiously across a piece of parchment.He mutters to himself as he writes, occasionally pausing to carefully correct a word or phrase that does not meet his exacting standards.


            As the players approach, Brother Conrad looks up briefly, his eyes flickering over their faces before returning to his work.He does not seem to register their presence, lost in his own world of ink and parchment.Only when they speak to him does he look up again, his eyes unfocused and distant as he struggles to engage with them.


            ""Ye-Yes, you wanted to talk to me ? To me ? I-I am Brother Conrad. Yes.""");


            if (scriptoriumVisited)
            {
                AppendMessage(new ChatMessage()
                {
                    Role = "assistant",
                    Content = "(You are back in the scriptorium with brother Conrad.)"
                });
            }
            else
            {
                AppendMessage(new ChatMessage()
                {
                    Role = "assistant",
                    Content = currentNPC.lastMessage()
                });
            }

            scriptoriumVisited = true;

        }

        private void Mortuary()
        {
            currentNPC = new NPC(globalContext, @"The player is currently in the mortuary talking with Brother Galeazzo.

			Ambiance :

			The air is filled with incense, the small body of Friedrich II is covered in a red and white flag, his hands a joined on his chest. Brother Galeazzo is quietly reading prayers at the deceased Duke. He barely inspected the body waiting for the player.

			Actions :

			If the player checks the body of Friedrich II von Habsburg :
			Respond that the body is shrouded in a red and white flag and ‘His hands are joined’ and his eyes are closed. Describe the lots of incense burning around him.

			If the player talks to Brother Galeazzo
			Respond that he barely checked the body and isn’t sure of the cause of death. He didn’t have any strange symptoms before death. The body was found in the garden by the Abbot.

			If the player ask Brother Galeazzo about the body
			Respond that the body clearly has signs of poisoning, but he was specifically told by the abbot not to examine it further before the players arrived, so he didn't and doesn't know more, but now that they are here is happy to help.

			If the player Checks the eyes and mouth of the corpse
			Respond that his eyes are completely red and his tongue is swollen. He was clearly poisoned but could be suicide.

			(need Knowledge (medicine) or (apothecary)) If the player analyse poison or medicine
			Respond that the poison is The Composition of Death, red copper, nitric acid, verdigris, arsenic, oak bark, rose water and black soot

			If the player open his hands or inspect his chest or remove the flag
			Respond that there is a massive scar on his chest, his heart is missing ! The doctor didn't notice this when he got the corpse. It was removed after his death. Clearly a murder !

			Remember : You will talk to me as you would to a player of this game and your answers should always be one paragraph long and descriptive in a very educated writing style.",
            @"As the players enter the mortuary, they immediately notice a strong smell of incense that permeates the air.

			The room is illuminated by flickering candles, casting eerie shadows on the stone walls and floor. The temperature in the room is frigid, making the players' breath visible in the air.

			The shrouded body of Friedrich II von Habsburg lies on a table in the center of the room, covered by a white and red flag that is emblazoned with the Habsburg coat of arms.

			The players can feel the weight of the solemnity in the air, as Brother Galeazzo moves around the room preparing the body for burial.

			The atmosphere is heavy and mournful, as if the room itself is holding its breath in reverence to the deceased. The players approach the table to inspect the body, feeling a sense of solemnity and respect for the young man who met such an untimely end.

			""Greetings, I am Brother Galeazzo, the abbey's doctor, how may I assist you ?""");


            if (mortuaryVisited)
            {
                AppendMessage(new ChatMessage()
                {
                    Role = "assistant",
                    Content = "(You are back in the mortuary with Brother Galeazzo.)"
                });
            }
            else
            {
                AppendMessage(new ChatMessage()
                {
                    Role = "assistant",
                    Content = currentNPC.lastMessage()
                });
            }

            mortuaryVisited = true;

        }

        private void Archives()
        {
            currentNPC = new NPC(globalContext, @"More Context :
			They are exploring the basement of the abbey of Neuberg, in the basement there's the archives of the Societa Templois, a knight order founded by Otto der Fröhliche and who fought against pagans in the far East.

			This order doesn’t exist anymore, it disappeared shortly after the death of Otto.

			As they explore the archives they see very weird books chained to the shelves as if they were beast ready to jump at them.
			Some of the books are really too thick some are really too thin or of weird proportions.
			Some have a perfectly black cover and some seems to be bound with human skin.
			There's a cage in the middle with a massive book with teeth. There are also armors, crests and weapons of the defeated pagans. They are of very weird and disturbing shapes.

			If the player wants to read a book
			Respond that the more they read the more they want to read and they start reading like madman until a monk pulls them out of the book.

			If the player persists on reading a book
			Respond by a quote of the content of the book. It is twisted, dark and illogical. Then describe that the player becomes mad. The quote is as follow 'In the darkest corners of the earth, beyond the reach of men, there lies a power that no mortal should seek. Its name is whispered only by the mad and the desperate, and those who speak it are cursed to an eternity of agony and despair.'

			If the player wants to open the massive cage
			Response that they can't because the cage is locked and nothing they do will unlock it.

			If the player wants to read the book with teeth
			Response that they can't because it is locked in the cage.

			Remember : You will talk to me as you would to a player of this game and your answers should always be one paragraph long and descriptive in a very educated writing style.",
            @"As you descend the stairs towards the Archives, the air grows colder and damp, and the sound of your footsteps echoes throughout the stone-walled corridor.

			The flickering torches lining the walls cast eerie shadows, revealing ancient cracks and crevices that have remained unseen for decades.

			You can feel the weight of history bearing down on you, as if the very stones themselves hold secrets of the past.

			Finally, you reach the bottom of the stairs and find yourself standing before the entrance to the archives of the Societa Templois, the order that fought against pagans in the far East.

			The door creaks as you push it open, revealing the dimly lit room before you, with its chained books and strange artifacts, waiting to be explored.");


            if (archivesVisited)
            {
                AppendMessage(new ChatMessage()
                {
                    Role = "assistant",
                    Content = "(You are back in the archives.)"
                });
            }
            else
            {
                AppendMessage(new ChatMessage()
                {
                    Role = "assistant",
                    Content = currentNPC.lastMessage()
                });
            }

            archivesVisited = true;
        }

        private void Crypt()
        {
            string canOpenTheTombs = clueMissingHeart ? "Respond that the monks agree to exhume the corpses of Otto der Fröhliche and Leopold II von Habsburg. Then the player see that their hearts are missing too ! It was removed recently apparently." : "Respond that this would be highly improper and the monks will never allow it.";

            string cryptPrompt = @"
            More Context:
            They are exploring the crypt of the abbey of Neuberg where Both Otto der Fröhliche and Leopold II von Habsburg are burried.


            It is a very solemn crypt with a massive statue of Otto der Fröhliche in armor, and it’s written below: Otto slaying the heretics. Lots of candles and gifts in front of him.One Monk is crying, this family was the abbey’s only protectors, what will happen now ?


            If the player wants to exhume or inspect the body of Otto der Fröhliche or Leopold II"+
            canOpenTheTombs
            + @"
            Remember: You will talk to me as you would to a player of this game and your answers should always be one paragraph long and descriptive in a very educated writing style.;";

           currentNPC = new NPC(globalContext, cryptPrompt,
            @"As you enter the solemn crypt of the Neuberg Abbey, a feeling of reverence descends upon you.
			The dimly-lit space is lined with ornate pillars and arches, giving it a distinctly medieval feel.
			At the center of the room stands a massive statue of Otto der Fröhliche in full armor, towering over the visitors.
			Below the statue, it is written ""Otto slaying the heretics"", a reminder of the late ruler's role in the holy military order.

            The floor is covered in intricate mosaics depicting various religious scenes.You notice that there are numerous candles and gifts placed in front of the statue, a sign of the reverence and devotion that the people of the abbey had for their protectors.

            As you look around, you see a monk crying in the corner, clearly distressed by the loss of this branch of the Habsburg family.");


            if (cryptVisited)
            {
                AppendMessage(new ChatMessage()
                {
                    Role = "assistant",
                    Content = "(You are back in the crypt.)"
                });
            }
            else
            {
                AppendMessage(new ChatMessage()
                {
                    Role = "assistant",
                    Content = currentNPC.lastMessage()
                });
            }

            cryptVisited = true;

        }

    }

    public class NPC
    {
        private OpenAIApi openai = new OpenAIApi();

        private List<ChatMessage> messages;

        public NPC(string globalContext, string persona, string welcomeMessage)
        {
            globalContext = globalContext.Replace("\t", "");
            persona = persona.Replace("\t", "");
            welcomeMessage = welcomeMessage.Replace("\t", "");

            messages = new List<ChatMessage>();

            messages.Add(new ChatMessage()
            {
                Role = "system",
                Content = globalContext + "\n" + persona
            });

            messages.Add(new ChatMessage()
            {
                Role = "assistant",
                Content = welcomeMessage
            });
        }

        public async Task<string> Say(string message)
        {
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = message
            };

            messages.Add(newMessage);


            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0301",
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var response = completionResponse.Choices[0].Message;
                response.Content = response.Content.Trim();

                messages.Add(response);
                return response.Content;
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
                return "";
            }

        }

        public string lastMessage()
        {
            return messages.Last().Content;
        }
    }
}
