Feature: CartInfoView
	In order to view the items in my cart
	As a user
	I want to get the item list of my cart
@mytag
Scenario: success  to view cart info with valid credential
	Given Register And Login And Create Cart
	When Cartinfo get products included
	Then should get  response of 'OK'

Scenario: fail  to view cart info with invalid credential
	Given Register And Login And Create Cart
	When CartInfo visit the cart info API with invalid credential
	Then should get  response of 'CredentialError'


	
