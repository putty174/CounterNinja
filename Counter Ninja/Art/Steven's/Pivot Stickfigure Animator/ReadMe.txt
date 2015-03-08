Pivot

Version 2.2

19 / 5 / 2004

Peter Bone

peterbone@hotmail.com
www.geocities.com/peter_bone_uk

Pivot MSN Group : http://groups.msn.com/Pivotanimation


Pivot makes it quick and easy to make simple stick-man animations. The animation can be saved as an animated gif, sequence of bitmaps or a native file so that they can be edited later. You can also build your own stick figures, save them and include them in your animation.


**********************************************************************


Things you can do that may not be obvious:

You can edit frames made previously by clicking on them.

If you right click on a frame a popup menu will appear which gives you the option of deleting or inserting a frame. The inserted frame will appear infront of the frame you clicked.

Rotating the mouse wheel steps through the stickmen. This is useful if one of the stickmen is outside the screen because you can rotate the mouse wheel until that stickman is highlighted and then click the center button. 

To reduce the aliasing (line jagedness) of the animation, make the animation big and then choose to reduce the frames in the gif options. The bigger the reduction the smoother the edges. 


**********************************************************************


Figure Builder 

Open the figure builder by clicking 'Create Figure Type' in the file menu of the main form or clicking the edit button to edit the current stick figure type. The edit mode option in the options menu allows you to change segment lengths when selected and normal pivot mode when not selected.


tool buttons (from top to bottom)

add line - add a line segment to the figure. Click the button, then click a handle on the figure that the segment will pivot on, then click somewhere on the image to set the end of the line.

add circle - add a circle segment to the figure. Add in the same way as above.

toggle segment kind - toggle the selected segment (blue) between line and circle.

change segment thickness - change the line thickness of the selected segment. Zero thickness segments will appear as invisible when not selected - these can be used to create figures with disconnected regions.

duplicate segment - duplicate a segment so that it has identical length/thickness. Select the segment you want to duplicate, then click the button, then click a handle on the figure to attach it.

static / dynamic segment - toggle the selected segment between static and dynamic. A dynamic segment will have a handle and will therefore be able to move in an animation, a static segment will not. Default is dynamic. A segment is shown as grey in the builder if it is static and black if it is dynamic (when it's not selected). Static segments can be rotated in the builder.

delete segment - delete the selected segment. Segments can only be deleted if they have no other segments attached to their endpoint.


**********************************************************************


Improvements since v1.3:

Stick figures can now be created, saved and loaded into an animation.

The animation now plays on the main form.

You can now have up to 256 stick figures per frame instead of just 10.

Piv files are smaller since they are now compressed.

Different colour stickmen.


Improvements since v2.1:

Gif optimization options are all selected by default.

Piv files are loaded faster since the timeline frames are re-drawn faster.

There are 2 new buttons to move the current figure in front or behind the other figures. This is useful when there are multiple figures with different colours.

Piv and Stk files can now be opened by dragging them over the pivot.exe icon or double clicking on them. See below for how to associate the files with pivot.

File save warnings added.


**********************************************************************


Bugs:

There is occasionally an error when saving as gif. Please make sure you save the animation as a .piv file first to ensure that the animation is not lost.
Parameter incorrect bug under windows 98.


**********************************************************************


Future development (some of these may never happen):

JPeg support for backgrounds.

Interchangable backgrounds.

There will be bitmap props. These props will be loaded as bitmaps with transparent backgrounds and it will be possible to rotate them and move them around (fluffy animals perhaps).

You'll be able to draw on the frames to include props or other details.

Frame delay setting for individual frames.

Tweening. The program will automatically create frames between begin and end positions of the stickmen. Like flash.

Stick figures are currently drawn on the same image which means that all the stickmen have to be redrawn when one is moved, which is slow. Each figure will be drawn on its own transparent overlay so that only that figure has to be redrawn. This should make it possible to have as many figures as you like. If only I could work out how to do it without causing flicker.

Automatic actions. This will make it easier to make stickmen do common actions such as walking or running - the program will automatically move the stickman.

Movements in 3 dimensions - but probably not because this will make the interface complicated.

Avi support. Also audio recording to accompany an animation.



**********************************************************************


Please tell me if you have any suggestions or if you have found a bug - and tell me what you did to create the error, don't just tell me you found a bug!