sudo apt install slapd ldap-utils
sudo apt install bind9
sudo apt-get install isc-dhcp-server
sudo systemctl start isc-dhcp-server
sudo systemctl enable isc-dhcp-server
sudo apt update 
sudo apt install apache2