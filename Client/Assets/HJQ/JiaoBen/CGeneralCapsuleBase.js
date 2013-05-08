/*
  CGeneralCapsuleBase类:
  说明：通用型延时胶囊，延迟一定时间生成若干个次级物体，以产生一个基于时间的事件链
  
  次级物体必须也是一个含有CGeneralCapsuleBase脚本的Prefab
*/
class CGeneralCapsuleBase extends MonoBehaviour
{
	public var ClusterPrefab:CGeneralCapsuleBase;			// 胶囊内含物质的Prefab名称
	public var ClusterCount:int = 3;						// 胶囊内物质的实例个数
	public var ClusterExplodeDelay:float = 3;				// 使胶囊内的物质的实例化过程延迟启动，单位为秒。也就是可以延迟喷出子物体。
	public var LifeTime:float = 5;							// 当前物体的生命周期的平均值，单位为秒
	public var LifeTimeMin:float = -1;						// 当前物体生命周期的随机浮动范围的下限
	public var LifeTimeMax:float = -1;						// 当前物体生命周期的随机浮动范围的上限
	public var InitialPosition:EnumInitialPosition = EnumInitialPosition.ParentPos;		// 下一级物体诞生位置的类型
	public var InitialPosParameter:Vector3 = Vector3(0,0,0);// 下一级物体诞生位置的参数,根据下一级物体诞生位置类型的不同,这个值代表不同的含义
	public var InitialPosRnd:Vector3 = Vector3(0,0,0);		// 下一级物体诞生位置的参数随机范围,仅当EnumInitialPosition中选择了随机类型的时候才起作用
	public var InitialRotation:Vector3 = Vector3(0,0,0);	// 下一级物体诞生的旋转角度
	
	protected var ArrClusterObj:CGeneralCapsuleBase[];		// 由ClusterPrefabName实例化出来的Prefab实体
	protected var trueExploded:boolean;						// 胶囊内物质是否已经爆发过一次(防止多次爆发)

	public enum EnumInitialPosition			// 初始出现位置的类型
	{
		ParentPos,			// 相对于父节点的位置
		WorldPos,			// 相对于世界中心的位置
		RndPosToParent,		// 相对于父节点的随机位置
		RndPosToWorld,		// 相对于世界中心的随机位置
	}
	
	function Awake()
	{
		trueExploded = false;
		ClampValue();
	}
	
	// ===继承于MonoBehaviour的方法
	function Start()
	{
		TicketToHell();
		ActivateClusterExplosion();
	}
	
	function Update()
	{
		UpdateSelfTransformation();
	}
	
	// 对特殊值作限位保护
	protected function ClampValue()
	{
		if (ClusterCount < 0)
		{
			ClusterCount = 0;
		}
		if (ClusterExplodeDelay < 0)
		{
			ClusterExplodeDelay = 0;
		}
		if (LifeTime<0)
		{
			LifeTime = 0;
		}
		InitialPosRnd = Vector3(Mathf.Abs(InitialPosRnd.x),Mathf.Abs(InitialPosRnd.y),Mathf.Abs(InitialPosRnd.z));
	}
	
	///////////////////////////////////////////////////////////////////////////////////
	// ===CGeneralCapsuleBase成员方法
	///////////////////////////////////////////////////////////////////////////////////

	// 根据延时爆发胶囊内物质
	protected function ActivateClusterExplosion()
	{
		yield WaitForSeconds(ClusterExplodeDelay);
		
		Explode();
	}
	
	// 根据延时将当前物体销毁
	protected function TicketToHell()
	{
		var l_LifeTime = LifeTime;
		
		// 仅当LifeTimeMin和LifeTimeMax在[0,LifeTimeMin]这个有效范围内时才作随机变化
		// *****注意对于LifeTime小于LifeTimeMin以及LifeTime大于LifeTimeMax这2种越限情况，
		// 为提高表现效果的自由度和判断执行代码的效率，目前不作限制，如有必要，可以在子类中覆盖TicketToHell()方法
		// 只需要加上两个附加条件(LifeTimeMin<=LifeTime)和(LifeTimeMax>=LifeTime)
		if ((LifeTimeMin >= 0)&&(LifeTimeMax >= LifeTimeMin))
		{
			var l_rnd:float = Random.Range(-0.5,0.5);
			if (l_rnd>0)
			{
				l_rnd *= (LifeTimeMax-LifeTime);
			}
			else
			{
				l_rnd *= (LifeTime-LifeTimeMin);
			}
			l_LifeTime += l_rnd;
		}
		yield WaitForSeconds(l_LifeTime);
		
	    Destroy(gameObject);
	}
	
	///////////////////////////////////////////////////////////////////////////////////
	// 以下为子类可继承接口
	///////////////////////////////////////////////////////////////////////////////////
	
	// 按照速度更新当前物体下一贞的位置
	protected function UpdateSelfTransformation()
	{
		// overridable....
	}
	
	// 实例化待爆发的粒子胶囊内容
	protected function SpawnAlgorithm()
	{
		// overridable....
		var i:int;
		var _SpawnPos:Vector3 = transform.position;
		var _rndVec3:Vector3;
		ArrClusterObj = new CGeneralCapsuleBase[ClusterCount];
		switch (InitialPosition)
		{
			case EnumInitialPosition.ParentPos:
				_SpawnPos += InitialPosParameter;
				for (i=0;i<ClusterCount;i++)
				{
					ArrClusterObj[i] = Instantiate(ClusterPrefab, _SpawnPos, Quaternion.identity.Euler(InitialRotation));
					//ArrClusterObj[i].transform.parent = this.transform;  // 不可以这样写，会导致父节点消失时,新生成的子节点也消失
				}
				break;
			case EnumInitialPosition.WorldPos:
				_SpawnPos = InitialPosParameter;
				for (i=0;i<ClusterCount;i++)
				{
					ArrClusterObj[i] = Instantiate(ClusterPrefab, _SpawnPos, Quaternion.identity.Euler(InitialRotation));
				}
				break;
			case EnumInitialPosition.RndPosToParent:
				_SpawnPos += InitialPosParameter;
				for (i=0;i<ClusterCount;i++)
				{
					_rndVec3 = Vector3(Random.Range(-InitialPosRnd.x,InitialPosRnd.x),Random.Range(-InitialPosRnd.y,InitialPosRnd.y),Random.Range(-InitialPosRnd.z,InitialPosRnd.z));
					ArrClusterObj[i] = Instantiate(ClusterPrefab, _SpawnPos+_rndVec3, Quaternion.identity.Euler(InitialRotation));
				}				
				break;
			case EnumInitialPosition.RndPosToWorld:
				_SpawnPos = InitialPosParameter;
				for (i=0;i<ClusterCount;i++)
				{
					_rndVec3 = Vector3(Random.Range(-InitialPosRnd.x,InitialPosRnd.x),Random.Range(-InitialPosRnd.y,InitialPosRnd.y),Random.Range(-InitialPosRnd.z,InitialPosRnd.z));
					ArrClusterObj[i] = Instantiate(ClusterPrefab, _SpawnPos+_rndVec3, Quaternion.identity.Euler(InitialRotation));
				}				
				break;
		}
	}
	
	protected function ExplosiveAlgorithm()
	{
		// overridable....
	}
	
	///////////////////////////////////////////////////////////////////////////////////
	// 以下为对外接口
	///////////////////////////////////////////////////////////////////////////////////

	// 爆发下一级物体
	public function Explode()
	{
		// 仅在未爆发过的状态下才作爆发
		if (!trueExploded)
		{
			if ((ClusterPrefab != null)&&(ClusterCount>0))
			{
				// 实例化待爆发的粒子胶囊内容
				SpawnAlgorithm();
				// 调用子类中的粒子胶囊内容的初始运动算法
				ExplosiveAlgorithm();
			}
			trueExploded = true;
			//Debug.Log("Exploded!");
		}
	}
}
