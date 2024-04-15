using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
	public string playerResponse;
	public string dialoguePrompt;
	public string fullResponse;
	public string dialougePrompt;
	Node decision1;
	Node decision2;

	public Node FetchNode(int branchNum)
	{
		Node current=null;
		if(pointers == null)return null;
		if (branchNum == 1) current=decision1;
		if (branchNum == 2) current=decision2;
		return current;
	}
	public Node SetNode(string buttonText, string prompt, string full)
	{
		if ((buttonText == null) || (prompt == null) || (full == null))return null;
		this.playerResponse=buttonText;
		this.dialougePrompt=prompt;
		this.fullResponse=full;
		decisions1=null;
		decisions2=null;
		return this;
	}
	public void ConnectNode(Node leaf)
	{
		if (decision1 == null)
		{
			decision1=leaf;
		}
		if (decision2 == null)
		{
			decision2=leaf;
		}
	}
	public string PrintButtonText()
	{
		return playerResponse;
	}
	public string PrintResponse()
	{
	}
}
