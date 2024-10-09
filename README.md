# Spotify
Connexion grâce à l'Authorization code flow

L'idée du projet est qu'un bar peut avoir plusieur porvider avec différent compte, par exemple il pourrait avoir deux employés qui ont chacun un compte spotify ça ferait deux provider spotify et l'un de pourrait ajouter son compte deezer.
On aurait donc 2 providers spotify et un deezer, tout ça sur le même bar.

url connexion à spotify --> https://accounts.spotify.com/authorize?response_type=code&client_id={clientId}&redirect_uri={redirectUri}&scope={scope}&state={barId}&prompt=login
