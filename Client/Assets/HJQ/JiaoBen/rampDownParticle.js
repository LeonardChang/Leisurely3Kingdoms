
var delayTime:float=0;			// 粒子发射前的延时
var delayPlusTime:float=0;		// 粒子发射后到开始衰减前的延时
var rampDownTime:float=1;		// 粒子衰减时间，在衰减时间内粒子数量线性减小
var origMinEmission:float;		// 粒子发射数下限的初始值(粒子初始的minEmission属性)
var origMaxEmission:float;		// 粒子发射数上限的初始值(粒子初始的maxEmission属性)

function Start () {
	origMinEmission=particleEmitter.minEmission;
	origMaxEmission=particleEmitter.maxEmission;
	particleEmitter.emit=false;
}

function Update () {
	if((delayTime+delayPlusTime)>0) delayTime-=Time.deltaTime;

	if(delayTime<=0 && particleEmitter.emit==false) particleEmitter.emit=true;

	if((delayTime+delayPlusTime)<=0){
		particleEmitter.minEmission=origMinEmission*rampDownTime;
		particleEmitter.maxEmission=origMaxEmission*rampDownTime;
		rampDownTime-=Time.deltaTime;
		if(rampDownTime<0){
			rampDownTime=0;
		}
	}
}
