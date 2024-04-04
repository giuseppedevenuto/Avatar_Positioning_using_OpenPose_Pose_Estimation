% plotta i punti che ci sono e quando mancano mantiene i punti precedenti
% fin quando non ci sarà un nuovo punto

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

v=VideoWriter('prova2_result_video.avi');
open(v);

x=[];
y=[];

for k = 1:nframes
    val = jsondecode(fileread(fullfile(framesdir,framenames{k})));

    xes = val.people.pose_keypoints_2d(1:3:end);
    yes = - val.people.pose_keypoints_2d(2:3:end);
    total=[xes, yes];

    if ~isempty(x)

        xes(xes==0)=x(xes==0);
        yes(yes==0)=y(yes==0);
        total=[xes, yes];

    end

    nonzeros_xes = nonzeros(xes);
    nonzeros_yes = nonzeros(yes);
    scatter(nonzeros_xes,nonzeros_yes,'.')
    x=xes;
    y=yes;

    daspect([1 1 1])
    ylim([-750 -150])
    xlim([600 1000])

    frame=getframe(gcf);
    writeVideo(v,frame);

end

close(v)