using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;

public class BradMemoryScript : MonoBehaviour {

	//globals
    public KMBombInfo bomb;
    public KMAudio Audio;

    //logging
    static int moduleIdCounter = 1;
    int moduleId;
	string[] movieNames = {"Thelma and Louise", "A River Runs Through It", "Se7en", "12 Monkeys", "Fight Club", "Ocean's 11", "Troy", "The Curious Case of Benjamin Button", "Inglorious Basterds", "Megamind", "Moneyball", "World War Z", "The Big Short", "Once Upon a Time in Hollywood", "Ad Astra", "Bullet Train", "Babylon", "Wolfs", "F1", "Pittsburgh", "Peach Pit", "Ball Pit"};

	private string[][] loggingFacts = {
		new string[]{"Thelma and Louise", "A River Runs Through It", "Se7en", "12 Monkeys", "Fight Club", "Ocean's 11", "Troy", "The Curious Case of Benjamin Button", "Inglorious Basterds", "Megamind", "Moneyball", "World War Z", "The Big Short", "Once Upon a Time in Hollywood", "Ad Astra", "Bullet Train", "Babylon", "Wolfs", "F1"},
		new string[]{"Romance Comedy","Drama","Action","Sci-Fi","Action","Action Comedy","Action Romance","Romance Drama","Action Comedy","Comedy","Sports Drama","Action Sci-Fi","Drama Comedy","Action Drama","Sci-Fi","Action Comedy","Drama Comedy","Action Comedy","Sports Action"},
		new string[]{"J.D.","Paul Maclean","David Mills","Jeffrey Goines","Tyler Durden","Rusty Ryan","Achilles","Benjamin Button","Aldo Raine","Metro Man","Billy Beane","Gerry Lane","Ben Rickert","Cliff Booth","Roy McBride","Ladybug","Jack Conrad","","Sonny Hayes"},
		new string[]{"1991","1992","1995","1995","1999","2001","2004","2008","2009","2010","2011","2013","2015","2019","2019","2022","2022","2024","2025"},
		new string[]{"Criminal","Fisher","Detective","Patient","Cult Leader","Con Man","Demigod","Human","Lieutenant","Superhero","General Manager","Survivor","Securities Trader","Stunt Double","Astronaut","Assassin","Actor","Fixer","Racer"},
		new string[]{"Ridley Scott","Robert Redford","David Fincher","Terry Gilliam","David Fincher","Steven Soderbergh","Wolfgang Petersen","David Fincher","Quentin Tarantino","Tom McGrath","Bennett Miller","Marc Forster","Adam McKay","Quentin Tarantino","James Gray","David Leitch","Damien Chazelle ","Jon Watts","Joseph Kasinski"},
		new string[]{"Susan Sarandon, Greena Davis","Craig Sheffer","Morgan Freeman, Gwyneth Paltrow, Kevin Spacey","Bruce Willis, Madeleine Stowe","Edward Norton, Helena Bonham Carter","George Clooney, Matt Damon, Al Pacino, Julia Roberts","Eric Bana, Orlando Bloom, Diane Kruger","Cate Blanchett","Christoph Waltz, Diane Kruger","Will Ferrell, Jonah Hill, Tina Fey","Jonah Hill, Philip Seymour Hoffman","Mireille Enos","Christian Bale, Steve Carell, Ryan Gosling","Leonardo DiCaprio, Margot Robbie","Tommy Lee Jones","Joey King, Bad Bunny","Margot Robbie, Diego Calva","George Clooney, Amy Ryan","Damson Idris, Kerry Condon, Javier Bardem"}
	};

	private string[] _loggingFacts = new string[7];

	//selectables
	public KMSelectable[] buttons;

	//renderers
	public MeshRenderer display;
	public MeshRenderer rim; 
	public MeshRenderer[] buttonLeds;
	public MeshRenderer [] stageLeds; 
	public TextMesh[] buttonNumbers; 
	public MeshRenderer background; 
	public GameObject dots; 
	public GameObject quest; 

	//options
	public Material[] dispMats;
	public Material[] rimMats; 
	public Material[] ledMats; 

	//movie facts
	private string[][] facts = {
		new string[]{"Thelma and Louise", "A River Runs Through It", "Se7en", "12 Monkeys", "Fight Club", "Ocean's 11", "Troy", "The Curious Case of Benjamin Button", "Inglorious Basterds", "Megamind", "Moneyball", "World War Z", "The Big Short", "Once Upon a Time in Hollywood", "Ad Astra", "Bullet Train", "Babylon", "Wolfs", "F1"},
		new string[]{"","1","A1","S1","A1","A","A","","A","1","S","AS","","A","S1","A","","A","AS"},
		new string[]{"less","","","","","less2","less","2","less","less2","2","less","","","","less","","less",""},
		new string[]{"1991","1992","1995","1995","1999","2001","2004","2008","2009","2010","2011","2013","2015","2019","2019","2022","2022","2024","2025"},
		new string[]{"Criminal","Fisher","Detective","Patient","Cult Leader","Con Man","Demigod","Human","Lieutenant","Superhero","General Manager","Survivor","Securities Trader","Stunt Double","Astronaut","Assassin","Actor","Fixer","Racer"},
		new string[]{"R","R","D1","T","D1","S","W","D1","Q1","T","B","M","A","Q1","J","D","D","J","J"},
		new string[]{"f5","5","3f5","f5","f","5f","3f5","f","f5","3f5","5","f","35","f","5","f","f5","f","3f5"}
	};

	private string[][] oriFacts = {new string[7],new string[7],new string[7],new string[7],new string[7]}; 
	private string[][] newFacts = {new string[7],new string[7],new string[7],new string[7],new string[7]};  

	//color mods
	private int[][] mods = {
		new int[]{1, 1, -2, 2, 0, -1, -4},
		new int[]{-2, 3, -7, -4, 1, 5, -1},
		new int[]{0, -1, 2, -3, 4, -5, 6},
		new int[]{-1, 1, -1, 1, -1, 1, -1},
		new int[]{-3, -2, -1, 0, 1, 2, 3},
		new int[]{0, 0, 0, 0, 0, 0, 0}
	};
	private int rimIndex; 
	private int[] rimHistory = new int[5]; 

	//order
	private int[][] orders = {
		new int[]{1, 3, 4, 2, 5},
		new int[]{2, 5, 3, 4, 1}, 
		new int[]{3, 1, 2, 5, 4},
		new int[]{4, 1, 5, 3, 2},
		new int[]{5, 4, 3, 1, 2}
	};
	int pressIndex = 0;
	private int[][] allPresses = new int[5][]; 
	private int[][] allPos = new int[5][]; 
	
	//other
	private int stage = 0; 
	private int movieIndex; 
	private int[] indexHistory = new int[5]; 
	private int[] numbers = {1, 2, 3, 4, 5}; 
	private List<int> _correctNumbers = new List<int>();
	private List<int> correctNumbers = new List<int>();
	private int val;
	private int pos;  
	List<int> currentPos = new List<int>(){0,0,0,0,0};
	bool waiting = false; 
	bool special = false;
	bool moduleSolved;
	int _stage; 
	

	void Awake(){
        moduleId = moduleIdCounter++;
		for (int i = 0; i < buttons.Length; i++){
			int j = i; 
			buttons[i].OnInteract += delegate () {ButtonPress(j); return false;};
		}
		/*/foreach (KMSelectable button in buttons){
            KMSelectable pressedButton = button;
            button.OnInteract += delegate () { ButtonPress(pressedButton); return false; };
        }/*/
	}

	void Start () {
			foreach (MeshRenderer buttonLed in buttonLeds){
				buttonLed.material = ledMats[0];
			}
			rim.gameObject.SetActive(true);
			display.gameObject.SetActive(true); 
			correctNumbers.Clear(); 
			_correctNumbers.Clear(); 
			currentPos.Clear(); 
			pressIndex = 0; 
			UpdateStage();
			RandomizeDisplays();
			if (stage == 5){
				quest.SetActive(true);
				dots.SetActive(false);
			}
			FindCorrectButtons();
			FindCorrectOrder();
	}

	void ButtonPress(int j){
		if (waiting || moduleSolved){
			return; 
		}
		string targetLed; 
		GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
		buttons[j].AddInteractionPunch(0.7f);
		val = Convert.ToInt32(buttons[j].GetComponentInChildren<TextMesh>().text); 
		switch (buttons[j].GetComponent<MeshRenderer>().material.name){
			case "1 (Instance)": 
				pos = 1; 
				targetLed = "1st";
			break; 
			case "2 (Instance)": 
				pos = 2; 
				targetLed = "2nd";
			break; 
			case "3 (Instance)": 
				pos = 3; 
				targetLed = "3rd";
			break;
			case "4 (Instance)": 
				pos = 4; 
				targetLed = "4th";
			break;
			case "5 (Instance)": 
				pos = 5; 
				targetLed = "5th";
			break; 
		}

		StartCoroutine(Depress(buttons[j]));
		if(_correctNumbers[pressIndex]==val){
			currentPos.Add(pos); 
			allPresses[stage-1][pressIndex] = val; 
			buttonLeds[j].material = ledMats[3];
			pressIndex++; 
		}
		else{
			buttonLeds[j].material = ledMats[4];
			StartCoroutine(Strike()); 
			Debug.Log("Incorrect Press"); 
		}
		
		List<int> _currentPos = currentPos.Where(i => i!=0).ToList();
		int[] __currentPos = _currentPos.ToArray(); 
		allPos[stage-1] = __currentPos; 

		if (pressIndex == _correctNumbers.Count){
			indexHistory[stage-1] = movieIndex; //index history for one stage logs as soon as that stage finishes, so don't know index history of current stage before completion
			if (stage == 5){
				StartCoroutine(Solved()); 
			}
			else{
				StartCoroutine(NewStage());
			}
		}
	}

	IEnumerator Solved(){
		waiting = true; 
		Audio.PlaySoundAtTransform("build", transform);
		for (int i = 4; i > -1; i--){
			StartCoroutine(SolveFade(buttons[i]));
			yield return new WaitForSeconds(0.1f);
			stageLeds[i].gameObject.SetActive(false); 
			yield return new WaitForSeconds(0.25f);
			buttons[i].gameObject.SetActive(false); 
		}
		display.gameObject.SetActive(false);
		dots.gameObject.SetActive(false); 
		quest.SetActive(false);
		Audio.PlaySoundAtTransform("boom",transform);
		background.material = dispMats[22];
		moduleSolved = true;
		GetComponent<KMBombModule>().HandlePass();
	}
	IEnumerator SolveFade(KMSelectable button){
		int it = 0; 
		float down = 0.0015f; 
		while (it < 20){
			button.transform.localPosition = new Vector3(button.transform.localPosition.x, button.transform.localPosition.y-down,button.transform.localPosition.z);
			it++; 
			yield return null; 
		}
		yield break; 
	}

	IEnumerator Strike(){
		waiting = true; 
		Audio.PlaySoundAtTransform("box", transform);
		int i = 0; 
		while (i < 5){
			yield return new WaitForSeconds(0.25f); 
			stageLeds[i].material = ledMats[4]; 
			i++;
		}
		yield return new WaitForSeconds(0.3f); 
		stage = 0; 
		for (int j = 0; j < oriFacts.Length; j ++){
			oriFacts[j] = new string[7]; 
		}
		for (int j = 0; j < newFacts.Length; j ++){
			newFacts[j] = new string[7]; 
		} 
		special = false; 
		GetComponent<KMBombModule>().HandleStrike();
		Start(); 
		waiting = false; 
	}

	IEnumerator Depress(KMSelectable button2){
		int it = 0; 
		float down = 0.001f; 
		while (it < 10){
			button2.transform.localPosition = new Vector3(button2.transform.localPosition.x, button2.transform.localPosition.y-down,button2.transform.localPosition.z);
			it++; 
			yield return null; 
		}
		float up = 0.0005f;
		while (it < 30){
			button2.transform.localPosition = new Vector3(button2.transform.localPosition.x, button2.transform.localPosition.y+up,button2.transform.localPosition.z);
			it++; 
			yield return null; 
		}
		yield break; 
	}
	IEnumerator NewStage(){
		waiting = true; 
		quest.SetActive(false); 
		display.gameObject.SetActive(false); 
		dots.SetActive(true);
		yield return new WaitForSeconds(0.5f); 
		int i = 0; 
		while (i < 3){
			dots.SetActive(false); 
			stageLeds[stage].material = ledMats[2]; 
			Audio.PlaySoundAtTransform("beep", transform);
			yield return new WaitForSeconds(0.15f); 
			stageLeds[stage].material = ledMats[3]; 
			dots.SetActive(true);
			yield return new WaitForSeconds(0.4f);
			i++;
		}
		display.gameObject.SetActive(true); 
		waiting = false; 
		quest.SetActive(true); 
		if (stage >3){
			dots.SetActive(false); 
		}
		Audio.PlaySoundAtTransform("thinking", transform);
		Start(); 
	}

	void UpdateStage(){
		stage++;
		for (int i = 0; i < stageLeds.Length; i++){
			stageLeds[i].material = ledMats[2];
		}
		for (int i = 0; i < stage; i ++){
			stageLeds[i].material = ledMats[3]; 
		}
	}

	void RandomizeDisplays(){
		if (stage == 5){
			rim.gameObject.SetActive(false);
			display.gameObject.SetActive(false); 
		}

		if (2 <= stage && stage <= 4 && !special){ 
			movieIndex = UnityEngine.Random.Range(0, dispMats.Length-2); 
			if (19 <= movieIndex && movieIndex <= 21){
				special = true; 
			}
		}

		else if (3 <= stage && stage <= 4 && !special){ 
			movieIndex = UnityEngine.Random.Range(0, dispMats.Length-1); 
			if (19 <= movieIndex && movieIndex <= 21){
				special = true; 
			}
		}

		else{
			movieIndex = UnityEngine.Random.Range(0, dispMats.Length-4); 
		}

		display.material = dispMats[movieIndex]; 
		numbers = numbers.OrderBy(x => Guid.NewGuid()).ToArray();
		for (int i = 0; i < buttons.Length; i++){
			buttonNumbers[i].text = numbers[i].ToString();
		}
		rimIndex = UnityEngine.Random.Range(0, rimMats.Length);
		rim.material = rimMats[rimIndex];
		rimHistory[stage-1] = rimIndex;
		
		if (stage != 5){
			Debug.LogFormat("Brad Memory #{0}: Stage {1} display movie is {2}", moduleId, stage, movieNames[movieIndex]);
			Debug.LogFormat("Brad Memory #{0}: Stage {1} rim color is {2}", moduleId, stage, rimMats[rimIndex].name);
		}
	}

	void FindCorrectButtons(){ //Movie, AS1, less2, year, role, letter1, 3f5
		if (19 <= movieIndex && movieIndex <= 21){
			switch (movieIndex){
				case 19:
					for (int i=0; i<7; i++){ //move down 9 from original facts from previous stage
						oriFacts[stage-1][i] = oriFacts[stage-2][i];
						newFacts[stage-1][i] = facts[i][(indexHistory[stage-2]+9)%19].ToString();
						_loggingFacts[i] = loggingFacts[i][(indexHistory[stage-2]+9)%19].ToString();
					}
					_stage = stage;
				break;
				case 20:
					for (int i=0; i<7; i++){ //use cyan rim with facts from Stage 1, instructions from stage 2
						oriFacts[stage-1][i] = oriFacts[0][i];
						newFacts[stage-1][i] = facts[i][(indexHistory[0]+19-mods[1][i])%19].ToString();
						_loggingFacts[i] = loggingFacts[i][(indexHistory[0]+19-mods[1][i])%19].ToString();
					}
					_stage = stage; 
					stage = 2; 
				break;
				case 21:
					for (int i =0; i<7; i++){
						int parallelIndex = 19-indexHistory[1]; 
						oriFacts[stage-1][i] = loggingFacts[i][parallelIndex];
						newFacts[stage-1][i] = facts[i][(parallelIndex+19-mods[rimHistory[0]][i])%19].ToString();
						_loggingFacts[i] = loggingFacts[i][(parallelIndex+19-mods[rimHistory[0]][i])%19].ToString();
					}
					_stage = stage; 
				break; 
			}
		}
		else{
			for (int i=0; i<7; i++){
				oriFacts[stage-1][i] = loggingFacts[i][movieIndex];
				newFacts[stage-1][i] = facts[i][(movieIndex+19-mods[rimIndex][i])%19].ToString();
				_loggingFacts[i] = loggingFacts[i][(movieIndex+19-mods[rimIndex][i])%19].ToString();
			}
			_stage = stage; 
		}
		
		if (stage != 5){
			Debug.LogFormat("Brad Memory #{0}: Stage {1} Original Facts are: {2}, {3}, {4}, {5}, {6}, {7}, {8}", moduleId, _stage, oriFacts[_stage-1][0], oriFacts[_stage-1][1], oriFacts[_stage-1][2], oriFacts[_stage-1][3], oriFacts[_stage-1][4], oriFacts[_stage-1][5], oriFacts[_stage-1][6]);
			Debug.LogFormat("Brad Memory #{0}: Stage {1} New Facts are: {2}, {3}, {4}, {5}, {6}, {7}, {8}", moduleId, _stage,  _loggingFacts[0], _loggingFacts[1], _loggingFacts[2], _loggingFacts[3], _loggingFacts[4], _loggingFacts[5], _loggingFacts[6]);
		}	
		switch (stage)
		{
			case 1:
				if (newFacts[_stage-1][1].Contains("a")||newFacts[_stage-1][1].Contains("A")){
					correctNumbers.Add(1);
					
				}
				if (newFacts[_stage-1][4].Contains("u")||newFacts[_stage-1][1].Contains("U")){
					correctNumbers.Add(2);
					
				}
				if (newFacts[_stage-1][6].Contains("3")){
					correctNumbers.Add(3);
					
				}
				if (Convert.ToInt32(newFacts[_stage-1][3])%2 == 0){
					correctNumbers.Add(4);
					
				}
				if (newFacts[_stage-1][2].Contains("less")||correctNumbers.Count == 0){
					correctNumbers.Add(5);
				
				}
			break;
			case 2:
				if (newFacts[_stage-1][0].Contains(" ") == false && newFacts[_stage-1][4].Contains(" ") == false){
					correctNumbers.Add(1);
				
				}
				if (allPresses[_stage-2].Contains(2)){
					correctNumbers.Add(2);
				
				}
				if ((Convert.ToDouble(newFacts[_stage-1][3])/3)%1 == 0 || allPresses[0].Length == 1){
					correctNumbers.Add(3); 
				
				}
				if (allPos[_stage-2].Contains(4)){
					correctNumbers.Add(4);
				
				}
				if (newFacts[_stage-1][6].Contains("f") || correctNumbers.Count == 0){
					correctNumbers.Add(5); 
				
				}

				if (movieIndex == 20){stage = _stage;}
			break;
			case 3:
				if (newFacts[_stage-1][1].Contains("1")&&allPos[0].Contains(5)){
					correctNumbers.Add(1);
				
				}
				if (newFacts[_stage-1][5].Contains("1")){
					correctNumbers.Add(2);
				
				}
				if (allPresses[0].Length + allPresses[1].Length > 3){
					correctNumbers.Add(3);
				
				}
				if (newFacts[_stage-1][6].Contains("5")){
					correctNumbers.Add(4);
			
				}
				if (Convert.ToInt32(newFacts[_stage-1][3]) < 2000 ||correctNumbers.Count == 0){
					correctNumbers.Add(5);
				
				}
			break;
			case 4:
				if (newFacts[0][0].Contains("b")||newFacts[0][0].Contains("B")){
					correctNumbers.Add(1);
					
				}
				if (newFacts[1][1].Contains("S")){
					correctNumbers.Add(2);
				
				}
				if (newFacts[2][2].Contains("2")){
					correctNumbers.Add(3);
				
				}
				char firstChar = newFacts[_stage-1][4][0];
				if (newFacts[0][5].Contains(firstChar)||newFacts[1][5].Contains(firstChar)||newFacts[2][5].Contains(firstChar)){
					correctNumbers.Add(4); 
					
				}
				if (indexHistory.Contains(movieIndex)||correctNumbers.Count == 0){
					correctNumbers.Add(5); 
					
				}
			break;
			case 5:
			int ones = 0; 
			for (int i = 0; i<4;i++){
				ones += allPresses[i].Count(x => x == 1);
			}
			if (ones == 1){
				correctNumbers.Add(1);
			
			}

			if (!allPos[1].Contains(2)&&!allPos[1].Contains(4)){
				correctNumbers.Add(2);
			
			}
			string vowels = "aeiouAEIOU"; 
			if ((rimHistory[2] == 1 || rimHistory[2] == 4)&&(vowels.Contains(newFacts[2][4][0])||vowels.Contains(newFacts[2][4][newFacts[2][4].Length-1]))){
				correctNumbers.Add(3); 
			
			}

			int fourths = 0; 
			for (int i = 0; i<4;i++){
				fourths += allPos[i].Count(x => x == 4);
			}
			string numbers = "0123456789"; 
			bool numberTitle = false; 
			for (int i = 0; i<4;i++){
				for (int j = 0; j < numbers.Length; j++){
					if (newFacts[i][0].Any(x => x == numbers[j])){
						numberTitle = true; 
					}
				}
			}
			if (fourths%2 == 0 || numberTitle == false){
				correctNumbers.Add(4); 
			
			}
			if (indexHistory.Any(x => 19<=x && x<=21)||correctNumbers.Count == 0){
				correctNumbers.Add(5); 
			
			}
			break;
		}
	}

	void FindCorrectOrder(){
		string __message = "null";
		if (movieIndex == 21){
			for (int i = 4; i > -1; i--){
				if (correctNumbers.Any(n => n == orders[stage-1][i])){ //checks in order if the order matches any of the correct presses
					_correctNumbers.Add(orders[stage-1][i]); //adds the order that matched in order of how they appear in the orders table 
				}
			}
		}
		else{
			for (int i = 0; i < 5; i++){
				if (correctNumbers.Any(n => n == orders[stage-1][i])){ //checks in order if the order matches any of the correct presses
					_correctNumbers.Add(orders[stage-1][i]); //adds the order that matched in order of how they appear in the orders table 
				}
			}
		}
		
		List<int> message = new List<int>(){0,0,0,0,0}; 
		for (int i=0; i < _correctNumbers.Count;i++){
			message[i]=_correctNumbers[i];
		}
		List<int> _message = message.Where(i => i!=0).ToList(); //takes out empty entries 
		int[] _array = new int[_correctNumbers.Count];   
		string[] array = _message.Select(i=> i.ToString()).ToArray();
		__message = String.Join(", ", array);
		_array = Array.ConvertAll(array, s => int.Parse(s));
		Debug.LogFormat("Brad Memory #{0}: Stage {1} correct pressing order (cardinal): {2}", moduleId, stage, __message);
		allPresses[stage-1] = _array; 
	}

	// Twitch Plays by Kilo Bites

#pragma warning disable 414
	private readonly string TwitchHelpMessage = @"!{0} position/pos/p 2 3 [presses button positions 2 and 3] || !{0} label/lab/l 4 5 [presses button positions labeled 4 and 5]";
#pragma warning restore 414

	IEnumerator ProcessTwitchCommand(string command)
	{
		string[] split = command.ToUpperInvariant().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

		if (waiting)
		{
			yield return "sendtochaterror You cannot interact with the module at this time!";
			yield break;
		}

		switch (split[0])
		{
			case "POSITION":
			case "POS":
			case "P":
			case "LABEL":
			case "LAB":
			case "L":
				if (split.Length == 1)
				{
					yield return "sendtochaterror Please specify what positions to press!";
					yield break;
				}
				if (split.Skip(1).Count() > 5)
				{
					yield return "sendtochaterror Too many parameters!";
					yield break;
				}
				if (split.Skip(1).Any(x => x.Length > 1))
				{
					yield return "sendtochaterror Make sure the digits you want to input are only in single digits.";
					yield break;
				}
				if (!split.Skip(1).Select(x => x[0]).All(char.IsDigit))
				{
					yield return string.Format("sendtochaterror {0} is/are not valid number(s)!", split.Skip(1).Select(x => x[0]).Where(x => !char.IsDigit(x)).Join(", "));
					yield break;
				}
				if (split.Skip(1).Count() > (_correctNumbers.Count - pressIndex))
				{
					yield return "sendtochaterror Too many parameters!";
					yield break;
				}
				if (split.Skip(1).Distinct().Count() != split.Skip(1).Count())
				{
					yield return "sendtochaterror There is a duplicate position/label in the order you want to press. Make sure they are unique!";
					yield break;
				}

				var isLabelCommand = new[] { "LABEL", "LAB", "L" }.Contains(split[0]);

				var buttonsToPress = split.Skip(1).Select(x => (isLabelCommand ? buttons.IndexOf(y => y.GetComponentInChildren<TextMesh>().text == x) : x[0] - '0' - 1)).ToArray();

				if (currentPos.Where(x => x != 0).Select(x => x - 1).Any(buttonsToPress.Contains))
				{
					yield return "sendtochaterror One or more buttons have already button pressed!";
					yield break;
				}
				if (!Enumerable.Range(0, 5).Any(buttonsToPress.Contains))
				{
					yield return "sendtochaterror Please make sure the button/label numbers are 1-5 inclusive!";
					yield break;
				}

				yield return null;

				foreach (var num in buttonsToPress)
				{
					buttons[num].OnInteract();
					yield return new WaitForSeconds(0.1f);
				}
				yield return "solve";
				yield break;
			default:
				yield return "sendtochaterror The command you inputted is invalid!";
				yield break;
		}
	}

	IEnumerator TwitchHandleForcedSolve()
	{
		while (waiting)
		{
			if (moduleSolved)
				yield break;

			yield return true;
		}

		while (!moduleSolved)
		{
			var correctButtonPresses = _correctNumbers.Where(x => !currentPos.Where(y => y != 0).Contains(x)).Select(x => buttons.IndexOf(y => int.Parse(y.GetComponentInChildren<TextMesh>().text) == x)).ToArray();

			foreach (var correctNum in correctButtonPresses)
			{
				buttons[correctNum].OnInteract();
				yield return new WaitForSeconds(0.1f);
			}

			while (waiting)
				yield return true;
		}
	}
}
