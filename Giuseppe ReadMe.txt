1. Estrarre openpose GPU

2. Estrarre i "models"(scaricati da un buon samaritano su internet dato che il server a cui si collegava il file .bat non funzionava)

3. Copiare il contenuto della cartella "models" appena estratta ("face","hand","pose") nella cartella "models" contenuta all'interno della cartella openpose

4. Invece di analizzare il video completo contenuto in ./example/media prendere in considerazione il video soldatino.mp4

5. Analizzare il video soldatino.mp4 usando i seguenti comandi nel prompt dei comandi con current folder "openpose":
 - per video:
	bin\OpenPoseDemo.exe --video soldatino.mp4 --display 0 --write_video examples/res_soldatino.avi --number_people_max 1 --write_json examples/result_soldatino_json

*(prende soldatino.mp4 e analizza una sola persona, non fa vedere il risultato (video più scheletro), ma lo salva come res_soldatino.avi (solo in formato .avi può farlo), e salva anche i punti nei files .json)*

[[ - per immagini:
	- C:\Users\Giuseppe\Desktop\openposegpu>bin\OpenPoseDemo.exe --image_dir examples/imm/ --display 0 --write_images examples/output --write_images_format jpg

	- C:\Users\Giuseppe\Desktop\openposegpu>bin\OpenPoseDemo.exe --image_dir examples/imm/ --hand --display 0 --render_pose 0 --write_json examples/output
]]
*(vai su internet per sapere cosa fa)*

6. Con i 3 codici matlab si possono ottenere dei video dei punti che si muovono nel tempo

7. In Unity importare l'avatar nel progetto

8. Se non si vede cliccare su Materials -> Selezionare tutto -> Shader (nell'inspector) -> Standard

9. Importare l'avatar nella scena

10. Creare "Animator" come "component" sull'avatar

11. Creare un "Animator Controller" in "Animation"

12. Trascinare il file appena creato nell' "Animator" dell'avatar

13. Nell'animator controller aggiungere un'animazione Idle come base (modificare l'animazione per far si che stia fermo l'avatar)

14. Seguire "Inverse Kinematics" tutorial di Unity