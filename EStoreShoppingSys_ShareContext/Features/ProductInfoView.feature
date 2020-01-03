﻿Feature: ProductInfoView
	In order to view the items in shop
	As a user
	I want to get the item list available
@mytag
Scenario: success  to view product info list with valid credential
	Given Register And Login And Create Cart
	When get product info list
	Then should get  response of 'OK'

Scenario: fail  to view cart info with invalid credential
	Given Register And Login And Create Cart
	When ProductInfo list get with invalid credential
	Then should get  response of 'CredentialError'


