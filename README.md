# Project Boost

## Technologies Used:

- C#
- Unity
- WebGL

## Description

The goal of Project Boost is to pilot you ship from the blue launch pad past the jutting, sometimes shifting, landscape and onto the yellow landing pad safely.

There is only 4 levels, but one mistake sends you back to the start !

## How I Did It

### Models

All of the models and landscape was created using Unity primitives that have be resized, rotated, and positioned to create the desired shapes. the terrain was made from a single prefab that I would duplicate and then adjust the size and shape.

The rocket was prefab was made up of multiple cubes, adjusted into different shapes. I also have a few particle systems on the rocket ship ready to be activated on thrust, success, or failure. There is also a spot light that will follow the rocket to assist with visibility, this was more necessary when I was playing with lighting levels.

### Gameplay

The main gameplay loop is very simple, go from point A to point B without touching the terrain. The only tools available to the player are the thrusters of the ship that allow it to move and turn. 

This simple gameplay was achieved entirely inside a Rocket.cs script on the rocket object. The only other script necessary was a simple Oscillator.cs used to make some terrain objects move.

In the Rocket.cs I used the OnCollisionEnter function on the rocket to check the tag of what the rocket touches. This way I could just set the tag of an object to "Friendly" or "Finish" if I wanted touching it to do nothing (not lose) or win the game respectively.