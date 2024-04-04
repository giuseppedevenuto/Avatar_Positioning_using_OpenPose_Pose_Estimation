% plotta i punti che ci sono

close all
clear
clc

filesdir = uigetdir(cd,'Seleziona la cartella con i *.json');

frames = dir(fullfile(filesdir,'*.json'));
framenames = {frames.name};
framesdir = frames.folder;
clear frames
nframes = length(framenames);

cd(fullfile('C:\Users\Giuseppe\Desktop\STHCI\0.Project\1.Matlab Preliminary Codes\matlab result videos'))

v=VideoWriter('prova1_result_video.avi');
open(v);

for k = 1:nframes
    val = jsondecode(fileread(fullfile(framesdir,framenames{k})));

    xes = nonzeros(val.people.pose_keypoints_2d(1:3:end));
    yes = - nonzeros(val.people.pose_keypoints_2d(2:3:end));

    scatter(xes,yes,'.')
    daspect([1 1 1])
    % ylim([-750 -150])
    % xlim([600 1000])

    frame=getframe(gcf);
    writeVideo(v,frame);

end

close(v)
cd ..