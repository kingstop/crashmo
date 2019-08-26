using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//		资源类型：http://blog.csdn.net/swj524152416/article/details/54022282

public enum EResourceType
{
	//	Prefab
	Prefab,

	//	Model
	FBX,

	//	Audio,
	MP3,
	WAV,
	OGG,

	//	Video
	mp4,

	//Texture
	png,
	jpg,
	dds,
	gif,
	psd,
	tga,
	bmp,

	//	data
	TXT,
	bytes,
	Xml,
	Csv,
	Json,

	//	AnimatorController,
	Controller,

	//	shader
	Shader,
	ShaderVariants,

	//	AnimationClip,
	Anim,

	Unity,

	//	Material,
	Mat,

	//	字体
	TTF,

	//	资源
	Asset,

	SkinnedMeshRender,

	Max,
}
