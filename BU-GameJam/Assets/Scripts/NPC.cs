using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;


public class NPC : MonoBehaviour
{

    public GameObject dialogPanel;
    public Text dialogText;
    public string[] dialog;
    private int index;
    
    public float wordSpeed;
    public bool playerIsClose;
    private bool hasStartedTalking = false;

    // Update is called once per frame
    void Update()
    {
        if(playerIsClose && !hasStartedTalking)
        {
            if(dialogPanel.activeInHierarchy)
            {
                zeroText();
            }
            else
            {
            	hasStartedTalking = true;
                dialogPanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }
    }
    
    public void zeroText()
    {
        dialogText.text = "";
        index = 0;
        dialogPanel.SetActive(false);
        StopAllCoroutines();
        hasStartedTalking = false;
    }
    
    IEnumerator Typing()
    {
    	dialogText.text = "";
        foreach(char letter in dialog[index].ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        
        yield return new WaitForSeconds(1.5f);
        
        NextLine();
        
    }
    
    public void NextLine()
    {
        if(index < dialog.Length -1)
        {
            index++;
            dialogText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            zeroText();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
    	if(other.CompareTag("Player"))
    	{
    	    playerIsClose = true;
    	}
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
    	if(other.CompareTag("Player"))
    	{
    	    playerIsClose = false;
    	    zeroText();
    	    //hasStartedTalking = false;
    	}
    }
    
}
