Feature: EditItemsInCart
	In order to select and discart the product as I like 
	As customer
	I want to add and delete product in the cart


Background:
	Given CARTADDITEM Register And Login And CreateCart
@mytag
Scenario: Success to Add Items into the cart
	Given CARTADDITEM add the items in table to cart
| itemId | quantity | price | amount    |
| 1      | 1        | 550   | 550       |
| 2      | 99       | 24.95 | 2470.05   |
| 3      | 999      | 22.5  | 22477.5   |
| 4      | 9999     | 14.99 | 149885.01 |
| 5      | 99999    | 27.9  | 2789972.1 |
	Then CARTADDITEM  should give  response of 'OK'
	And CARTADDITEM items in cart should be same to the table
	When CARTADDITEM delete all the items from cart
	Then CARTADDITEM  cart should be empty

Scenario: Should fail to add items to cart with invalid credential
Given CARTADDITEM add the valid items table to cart with invalid credential
| itemId | quantity | price | amount    |
| 1      | 1        | 550   | 550       |
Then CARTADDITEM  should give  response of 'TokenError'

Scenario: Should fail to add items to cart with invalid itemid
Given CARTADDITEM add the items in table to cart
| itemId	| quantity | price | amount    |
| 999999	| 1        | 550   | 550       |
Then CARTADDITEM  should give  response of 'InvalidItemError'

Scenario: Should fail to add items to cart with invalid cartid
Given CARTADDITEM add the items in table to cart with invalid cartid
| itemId	| quantity | price | amount    |
| 1	| 1        | 550   | 550       |
Then CARTADDITEM  should give  response of 'InvalidCartError'

Scenario: Should fail to delete items from cart with invalid credential
Given CARTADDITEM delete the valid items table from cart with invalid credential
| itemId | quantity | price | amount    |
| 1      | 1        | 550   | 550       |
Then CARTADDITEM  should give  response of 'TokenError'



Scenario: Should fail to delete items from cart with invalid itemid
	Given CARTADDITEM add the items in table to cart
| itemId | quantity | price | amount    |
| 1      | 1        | 550   | 550       |
And  CARTADDITEM delete the items in table to cart
| itemId	| quantity | price | amount    |
| 2			| 1        | 550   | 550       |
Then CARTADDITEM  should give  response of 'InvalidItemError'

Scenario: Should fail to check cart info with invalid credential
	Given CARTADDITEM add the items in table to cart
| itemId | quantity | price | amount    |
| 1      | 1        | 550   | 550       |
When CARTADDITEM check cart info with invalid credential
Then CARTADDITEM  should give  response of 'TokenError'