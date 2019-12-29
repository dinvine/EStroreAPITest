Feature: CartInfoView
	In order to view the items in my cart
	As a user
	I want to get the item list of my cart
@mytag
Scenario: success  to view cart info with valid credential
	Given CartInfo Register And Login And CreateCart
	When CartInfo visit the cart info API with valid credential
	Then CartInfo should give  response of 'OK'

Scenario: fail  to view cart info with invalid credential
	Given CartInfo Register And Login And CreateCart
	When CartInfo visit the cart info API with invalid credential
	Then CartInfo should give  response of 'TokenError'

Scenario: fail  to view cart info with invalid cartid
	Given CartInfo Register And Login And CreateCart
	When CartInfo visit the cart info API with invalid accountNumber
	Then CartInfo should give  response of 'accountNumberError'
	# the API give  response of 'ok' instead ! 
