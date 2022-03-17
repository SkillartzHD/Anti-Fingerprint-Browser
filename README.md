# Anti-Fingerprint-Browser


Majoritatea extensiilor pentru browser care se ocupa de a emula un ID unic este total aleatorie si ea nu poate stoca/edita respectiva informație , astfel la fiecare accesare a unui link/website/buton avem cate o noua amprenta digitala ceea ce poate para suspect pentru un anume website pentru ca nici un utilizator nu își poate schimba amprenta digitala mai ales ea fiind dinamica la orice buton apăsat in pagina.
Însă când vine vorba de o extensie pentru browserul web pentru a putea împiedica generarea amprentei digitale unice in unele cazuri poate fi eronata si scurgerea de informații se poate face chiar prin respectiva extensie prin simplu motiv ca extensia executa bucăți de cod in browser prin JavaScript ,  astfel extensia  doar ne ascunde un set de date din browser temporar însă aceasta rescriere poate sa fie incompleta/nelegitima si chiar ne poate face unici prin simplu fapt ca nimeni nu are o funcție standard modificata , inclusiv website-ul poate vedea ce fel de extensie folosim pe browser. 
Soluția pe care urmează sa o prezint nu executa nici un cod javascript in browser sau o extensie , totul se face la baza browserului.

Atunci când folosim versiunea oficiala de Selenium trebui sa folosim chromedriver(nucleul principal pentru automatizare) el vine la pachet cu un set de date incomplete si specifice pentru a putea fi identificat (de exemplu  navigator.webdriver fiind setat pe valoarea true când pornește automatizare ceea ce reprezintă ca browserul executa un test pe când browserul deschis normal are valoarea stabilita pe false ) însă exista o lista foarte mare de lipsuri atunci browserul este pornit cu Selenium.
Astfel noi daca încercam forțat sa punem valoarea pe false nu avem nici un rezultat pentru ca browserul setează la valoarea inițiala cu care a fost pornit.

![Picture1](https://user-images.githubusercontent.com/8433325/158842216-316c87ae-3fe8-4f73-a6dd-a31e9fd0e8cb.png)

Astfel o sa folosim o versiune neoficiala (ea se mai numește si ungoogled chromium), editata de o companie care are ca scop comercializarea in care inițial erau într-o strânsă legătura cu serverele lor pentru a putea genera o amprenta digitala pe serverele lor într-un mod criptat , însă după o analiza ampla a fișierelor din versiunea modificata am descoperit ca se poate face si într-un mod total diferit fără a interacționa cu api-ul lor.

Pentru a putea analiza fișierul de baza care se ocupa cu generarea de amprenta in browser ( fișierul se numește net.dll) am folosit programul IDA Freeware.

In fișierul net.dll avem următoarele argumente într-o funcție principala atunci când pornim browserul:
![Picture2](https://user-images.githubusercontent.com/8433325/158842322-46b6843f-17df-4a18-87df-3810e11db6d0.png)

*“fontset”* - ne ajuta sa setam o lista de fonturi in browser fără a le instala

“encodeddata” – reprezintă funcția pe care nu o putem apela din cauza ca exista o criptare a informaților care se face pe un server la care nu avem acces , însuși funcția respectiva doar decodează acel set de informații primit de serverul lor .
![Picture3](https://user-images.githubusercontent.com/8433325/158842550-e57b29e7-9e60-4f73-9e28-01ed75deef30.png)

*sub_18013E5C4* =  reprezintă funcția atunci când “encodeddata” este adăugat ca argument la pornirea browserului in care se ocupa cu decodarea informației respective care arată cam așa , deși într-un final daca decodarea este conforma atunci este apelata funcția din “base64” daca nu atunci nu funcționează.


![Picture4](https://user-images.githubusercontent.com/8433325/158842604-d07bcf38-7ad7-4513-9877-acd0613583b2.png)

Unde ne este confirmata prezentei criptări prin o parola setata pe server si una in client când este generata, ea se numește Salt (Salt cryptography)

“base64” – face exact ce ar trebui sa facă encodeddata cu excepția interacțiuni cu serverul extern si cu o criptare ușor înțeleasa , prin base64 se face codarea si decodarea informației respective , deci prin argumentul “base64” putem modela orice amprenta digitala in browser după un set de reguli stabilit de modul.


![Untitled](https://user-images.githubusercontent.com/8433325/158846119-f2497d32-5abb-47e9-9719-617dc205a89f.png)


*sub_18013E346*  = reprezintă funcția cheie pe care o putem folosi fără encriptia anterioara , care la rândul ei apelează funcția care ne interesează pentru a modela structura amprentei digitale in browser 

![Picture6](https://user-images.githubusercontent.com/8433325/158842706-a68141d5-5ac1-436e-8265-f565f52d2cc3.png)

*sub_18013E842* =  aceasta e funcția principala unde trebui sa respectam acel set de informații oferit care arată cam așa

![Picture7](https://user-images.githubusercontent.com/8433325/158842773-81e5b272-5f66-40c3-a5f2-5e2877c32bd2.png)

Chenarul roșu reprezintă funcția(*sub_18013E842* ) din care parte sunt aceste string-uri pe care trebui sa le legam cu o anume valoare si apoi legate într-un string final care trece prin codarea lui prin base64 , atunci putem trimite prin argumentul “base64=” codul rezultat prin codare si el o sa fie automat citit si decodat de funcția respectiva.

Chenarul negru reprezintă argumentele pe care trebui sa le legam pe toate într-un string

![Picture8](https://user-images.githubusercontent.com/8433325/158842877-b548d6f7-b49b-49c3-988c-373ae6a83fb5.png)



## Selenium result Browser-Fingerprint

incolumitas 1             |  incolumitas 2 
:-------------------------:|:-------------------------:
<img src="https://user-images.githubusercontent.com/8433325/158828890-732bed4c-b7ec-4c3a-8f0f-0f7c401c68af.png" width="100" height="100"> |  <img src="https://user-images.githubusercontent.com/8433325/158828907-2709d0a2-04dc-41df-a82e-0f91f7584f9b.png" width="100" height="100">


CreepJS 1               |  CreepJS 2
:-------------------------:|:-------------------------:
<img src="https://user-images.githubusercontent.com/8433325/158828929-bd060558-f30b-4e82-8bd7-ed5405d17036.png" width="100" height="100"> |  <img src="https://user-images.githubusercontent.com/8433325/158828939-5683356d-a4be-4aa3-9217-a7bb623801c6.png" width="100" height="100">



### GUI


https://user-images.githubusercontent.com/8433325/158841646-d3682907-2715-4b46-9b2a-375c93e2c419.mp4


