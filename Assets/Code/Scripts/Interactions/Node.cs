using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
	public string playerResponse;
	public string dialoguePrompt;
	public string fullResponse;
	public string dialougePrompt;
	public LinkedList<Node> pointers;

	public Node FetchNode(int branchNum)
	{
		Node current=null;
		if(pointers == null)return null;
		//if (branchNum == 1) current=this.pointers.Find(leaf1);
		//if (branchNum == 2) current=this.pointers.Find(leaf2);
		return current;
	}
	public Node SetNode(string buttonText, string prompt, string full)
	{
		if ((buttonText == null) || (prompt == null) || (full == null))return null;
		this.playerResponse=buttonText;
		this.dialougePrompt=prompt;
		this.fullResponse=full;
		pointers=null;
		return this;
	}
	public void ConnectNode(Node leaf)
	{
		if (pointers == null)
		{
			pointers= new LinkedList<Node>();
		}
		this.pointers.AddLast(leaf);
	}
	public string PrintButtonText()
	{
		return playerResponse;
	}
	public string PrintResponse()
	{
	}
}
