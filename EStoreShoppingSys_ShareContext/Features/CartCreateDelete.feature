Feature: CreateAndDeleteCart
	In order to begin and finish the shopping
	As user
	I want to create and delete the cart

@Smoke
Scenario: C18 success to create cart by valid token
	Given register login and  get valid token
	When CART create with the  token
	Then should get  response of 'OK'
	And should get response comform with model 'CartCreate'
	And add  item:  ['datas'] ['cartId,amountDue'] in response body to scenario context
	And should keep value constant for keys 'accountNumber'

@ShouldFail
Scenario: C19 fail to create cart by invalid token
	Given register login and  get valid token
	When CART create  with the  invalid token
	Then should get  response of 'CredentialError'
@ShouldFail
Scenario: C20 fail to create cart if user already has a cart
	Given register login and  get valid token
	When CART create with the  token twice
	Then should get  response of 'DuplicateCartError'

@Smoke
Scenario: C21 success to delete cart by valid token
	Given register login and  get valid token
	And CART create with the  token
	When CART delete  with the  token
	Then should get  response of 'OK'
	And should get response comform with model 'CartDelete'
	And add  item:  ['datas'] ['cartId,accountNumber'] in response body to scenario context
	And should keep value constant for keys 'cartId,accountNumber'

@ShouldFail
Scenario: C22 fail to delete cart by invalid token
	Given register login and  get valid token
	And CART create with the  token	
	When CART delete  with the invalid token
	Then should get  response of 'CredentialError'