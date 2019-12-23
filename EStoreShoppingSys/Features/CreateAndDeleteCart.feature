Feature: CreateAndDeleteCart
	In order to begin and finish the shopping
	As user
	I want to create and delete the cart

@mytag
Scenario: success to create cart by valid token
	Given get the valid token on 'TokenEndPoint'
	When create cart at 'CartCreateEndPoint' with the  token
	Then Cart  should give  response of 'OK'
	And CartAdd should give json with 'datas' containing items 'cartId,accountNumber,amountDue'

Scenario: fail to create cart by invalid token
	Given get the invalid token on 'TokenEndPoint'
	When create cart at 'CartCreateEndPoint' with the  token
	Then Cart  should give  response of 'TokenError'

Scenario: success to delete cart by valid token
	Given get the valid token on 'TokenEndPoint'
	And create cart at 'CartCreateEndPoint' with the  token
	When delete cart at CartDeleteEndPoint with the  token
	Then Cart  should give  response of 'OK'
	And CartDelete should give json with 'datas' containing items 'cartId,accountNumber'

Scenario: fail to delete cart by invalid token
	Given get the invalid token on 'TokenEndPoint'
	And create cart at 'CartCreateEndPoint' with the  token	
	When delete cart at 'CartDeleteEndPoint' with the invalid token
	Then Cart  should give  response of 'TokenError'