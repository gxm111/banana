using UnityEngine;
using System;
using System.Collections;

internal static class AnimationExtensions  {

public static IEnumerator Play( this Animation animation, string clipName, bool useTimeScale, Action<bool> onComplete )
 {
 //We Don't want to use timeScale, so we have to animate by frame..
 if(!useTimeScale)
 {
 AnimationState _currState = animation[clipName];
     bool isPlaying = true;
     //float _startTime = 0F;
     float _progressTime = 0F;
     float _timeAtLastFrame = 0F;
     float _timeAtCurrentFrame = 0F;
     float deltaTime = 0F;


 animation.Play(clipName);

 _timeAtLastFrame = Time.realtimeSinceStartup;
         while (isPlaying) 
 		{
             _timeAtCurrentFrame = Time.realtimeSinceStartup;
             deltaTime = _timeAtCurrentFrame - _timeAtLastFrame;
             _timeAtLastFrame = _timeAtCurrentFrame; 

                _progressTime += deltaTime;
                _currState.normalizedTime = _progressTime / _currState.length; 
                animation.Sample();


                if (_progressTime >= _currState.length) 
				{
					if(_currState.wrapMode != WrapMode.Loop)
				{
				                 isPlaying = false;
				}
				else
				{
					_progressTime = 0.0f;
				}
                }

             yield return new WaitForEndOfFrame();
         }
         yield return null;
 if(onComplete != null)
 {
 onComplete(true);
 } 
 }
 else
 {
 animation.Play(clipName);
 }
 }
	
public static IEnumerator Reverse( this Animation animation,string clipName, bool useTimeScale, Action<bool> onComplete )
 {
		if(!useTimeScale)
 {
 AnimationState _currState = animation[clipName];
     bool isPlaying = true;
     float _progressTime =0;
     float _timeAtLastFrame = 0F;
     float _timeAtCurrentFrame = 0F;
     float deltaTime = 0F;

 

 	
		 animation.Play(clipName);
 _timeAtLastFrame = Time.realtimeSinceStartup;
		 while (isPlaying) 
 		{
             _timeAtCurrentFrame = Time.realtimeSinceStartup;
             deltaTime = _timeAtCurrentFrame - _timeAtLastFrame;
             _timeAtLastFrame = _timeAtCurrentFrame; 

                _progressTime += deltaTime;
			animation.Play ();
                _currState.normalizedTime =1-( _progressTime / _currState.length); 
                animation.Sample ();
			animation.Stop ();

                if (_progressTime >= _currState.length) 
				{
					if(_currState.wrapMode != WrapMode.Loop)
				{
				                 isPlaying = false;
				}
				else
				{
					_progressTime = 0.0f;
				}
                }

             yield return new WaitForEndOfFrame();
         }
   
    yield return null;
 if(onComplete != null)
 {
 onComplete(true);
 } 
		}
		else{
		
		}
 
	}
}
