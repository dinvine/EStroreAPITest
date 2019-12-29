Feature: ProductInfoView
	In order to view the items in shop
	As a user
	I want to get the item list available
@mytag
Scenario: success  to view product info list with valid credential
	Given ProductInfo Register And Login And CreateCart
	When ProductInfo visit the cart info API with valid credential
	Then ProductInfo should give  response of 'OK'

Scenario: fail  to view cart info with invalid credential
	Given ProductInfo Register And Login And CreateCart
	When ProductInfo visit the cart info API with invalid credential
	Then ProductInfo should give  response of 'TokenError'


