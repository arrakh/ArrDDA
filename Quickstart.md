# Basic Usage Example

In this section, you can use these example as a basic training and basis that may be useful for your own usage.  
This example uses a *__Tangram Block Game__* . It is a puzzle game where you solve a puzzle using given *Tetris-like* pieces into an area.  
There are timer, block pieces and a grid inside of a level. Players need to finish the level by placing all the block pieces into a prefect fit.

## Preparation and Initialization

First, we need to define what we needed in our tangram block game. Then, we need to find values or parameters that can be used for our dynamic difficulty adjustment.  

In this game, the mechanics tha can be dynamically adjusted are :  

- Grid size
- Total block pieces, and
- Level timer

Second, we need to create *Channel* and *Metric* parameters. These parameters are used to define values that we can control using ArrDDA and can be dynamically adjusted.

![Right Click](/img/right_click.png)

To create these parameters, Right click your empty area in the project section folder in unity and choose *Metrics* or *Channel* accordingly. In these example, these are the *Channel* and *Metric* parameters created :  

- Score Metric
- Target block pieces Metric
- Target grid size Metric
- Target block pieces Channel
- Target grid size Channel

## Scripted Game Objects

## Additional Codes

## Testing