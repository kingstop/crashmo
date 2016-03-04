#pragma once
#ifndef _LOCATIONVECTOR_H
#define _LOCATIONVECTOR_H#include <math.h>///////////////////////////////////////////////////////////
// Location vector class (X, Y, Z, O)
//////////////////////////////////////////////////////////class LocationVector
{
public:	LocationVector(float X, float Y, float Z) : x(X), y(Y), z(Z), o(0) {}
	LocationVector(float X, float Y, float Z, float O) : x(X), y(Y), z(Z), o(O) {}
	LocationVector() : x(0), y(0), z(0), o(0) {}
	LocationVector( const LocationVector& lv ) : x( lv.x ), y( lv.y ), z( lv.z ), o( lv.o ) {}	LocationVector Center( const LocationVector& target ) const
	{
		LocationVector lv( *this );
		lv.x += (target.x - x) / 2.f;
		lv.y += (target.y - y) / 2.f;
		lv.z += (target.z - z) / 2.f;
		return lv;
	}	float DistanceSq(const LocationVector & comp)
	{
		float delta_x = comp.x - x;
		float delta_y = comp.y - y;
		float delta_z = comp.z - z;		return (delta_x*delta_x + delta_y*delta_y + delta_z*delta_z);
	}	float DistanceSq(const float &X, const float &Y, const float &Z)
	{
		float delta_x = X - x;
		float delta_y = Y - y;
		float delta_z = Z - z;		return (delta_x*delta_x + delta_y*delta_y + delta_z*delta_z);
	}	float Distance(const LocationVector & comp)
	{
		float delta_x = comp.x - x;
		float delta_y = comp.y - y;
		float delta_z = comp.z - z;		return sqrtf(delta_x*delta_x + delta_y*delta_y + delta_z*delta_z);
	}

	float Distance(const float &X, const float &Y, const float &Z)
	{
		float delta_x = X - x;
		float delta_y = Y - y;
		float delta_z = Z - z;
		return sqrtf(delta_x*delta_x + delta_y*delta_y + delta_z*delta_z);
	}
	float Distance2DSq(const LocationVector & comp)
	{
		float delta_x = comp.x - x;
		float delta_y = comp.y - y;
		return (delta_x*delta_x + delta_y*delta_y);
	}
	float Distance2DSq(const float & X, const float & Y)
	{
		float delta_x = X - x;
		float delta_y = Y - y;
		return (delta_x*delta_x + delta_y*delta_y);
	}

	float Distance2D(const LocationVector & comp)
	{
		float delta_x = comp.x - x;
		float delta_y = comp.y - y;
		return sqrtf(delta_x*delta_x + delta_y*delta_y);
	}
	float Distance2D(const float & X, const float & Y)
	{
		float delta_x = X - x;
		float delta_y = Y - y;
		return sqrtf(delta_x*delta_x + delta_y*delta_y);
	}

	float CalcAngTo(const LocationVector & dest)
	{
		float dx = dest.x - x;
		float dy = dest.y - y;
		if(dy != 0.0f)
			return atan2(dy, dx);
		else
			return 0.0f;
	}

	float CalcAngFrom(const LocationVector & src)
	{
		float dx = x - src.x;
		float dy = y - src.y;
		if(dy != 0.0f)
			return atan2(dy, dx);
		else
			return 0.0f;
	}

	void ChangeCoords(float X, float Y, float Z, float O)
	{
		x = X;
		y = Y;
		z = Z;
		o = O;
	}

	void ChangeCoords(float X, float Y, float Z)
	{
		x = X;
		y = Y;
		z = Z;
	}

	LocationVector & operator += (const LocationVector & add)
	{
		x += add.x;
		y += add.y;
		z += add.z;
		o += add.o;
		return *this;
	}

	LocationVector & operator -= (const LocationVector & sub)
	{
		x -= sub.x;
		y -= sub.y;
		z -= sub.z;
		o -= sub.o;
		return *this;
	}
	LocationVector & operator = (const LocationVector & eq)
	{
		x = eq.x;
		y = eq.y;
		z = eq.z;
		o = eq.o;
		return *this;
	}

	bool operator == (const LocationVector & eq)
	{
		if(eq.x == x && eq.y == y && eq.z == z)
			return true;
		else
			return false;
	}	float x;
	float y;
	float z;
	float o;
};

#endif
