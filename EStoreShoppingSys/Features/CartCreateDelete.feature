Feature: CreateAndDeleteCart
	In order to begin and finish the shopping
	As user
	I want to create and delete the cart

@mytag
Scenario: success to create cart by valid token
	Given CART get the valid token on 'TokenEndPoint'
	When CART create with the  token
	Then CART  should give  response of 'OK'
	And CART should give json with 'datas' containing items 'cartId,accountNumber,amountDue'

Scenario: fail to create cart by invalid token
	Given CART get the valid token on 'TokenEndPoint'
	When CART create  with the  invalid token
	Then CART  should give  response of 'TokenError'

Scenario: fail to create cart if user already has a cart
	Given CART get the valid token on 'TokenEndPoint'
	And CART create with the  token
	When CART create with the  token
	Then CART  should give  response of 'DuplicateCartError'


Scenario: success to delete cart by valid token
	Given CART get the valid token on 'TokenEndPoint'
	And CART create with the  token
	When CART delete  with the  token
	Then CART  should give  response of 'OK'
	And CART should give json with 'datas' containing items 'cartId,accountNumber'

Scenario: fail to delete cart by invalid token
	Given CART get the valid token on 'TokenEndPoint'
	And CART create with the  token	
	When CART delete  with the invalid token
	Then CART  should give  response of 'TokenError'