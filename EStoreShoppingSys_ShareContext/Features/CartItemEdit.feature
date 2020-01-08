Feature: EditItemsInCart
	In order to select and discart the product as I like 
	As customer
	I want to add and delete product in the cart


Background:
	Given Register And Login And Create Cart
@mytag
Scenario: Success to Add Items into the cart
	Given add the valid items table to cart
| itemId | quantity | price | amount    |
| 1      | 1        | 550   | 550       |
| 2      | 99       | 24.95 | 2470.05   |
| 3      | 999      | 22.5  | 22477.5   |
| 4      | 9999     | 14.99 | 149885.01 |
| 5      | 99999    | 27.9  | 2789972.1 |
	Then should get  response of 'OK'
	And should get response comform with model 'CartItemAdd'
	And add  item:  ['datas'] ['cartId,accountNumber'] in response body to scenario context
	And should keep value constant for keys 'cartId,accountNumber'
	And CartInfo items in cart should be same to the table
| itemId | quantity | price | amount    |
| 1      | 1        | 550   | 550       |
| 2      | 99       | 24.95 | 2470.05   |
| 3      | 999      | 22.5  | 22477.5   |
| 4      | 9999     | 14.99 | 149885.01 |
| 5      | 99999    | 27.9  | 2789972.1 |


Scenario: Should fail to add items to cart with invalid credential
Given CARTADDITEM add the valid items table to cart with invalid credential
| itemId | quantity | price | amount    |
| 1      | 1        | 550   | 550       |
Then should get  response of 'CredentialError'

Scenario: Should fail to add items to cart with invalid itemid
Given CARTADDITEM add the invalid items table to cart
| itemId	| quantity | price | amount    |
| 999999	| 1        | 550   | 550       |
Then should get  response of 'InvalidItemError'

Scenario: Should fail to add items to cart with invalid cartid
Given CARTADDITEM add the items in table to cart with invalid cartid
| itemId	| quantity | price | amount    |
| 1	| 1        | 550   | 550       |
Then should get  response of 'InvalidCartError'


Scenario: Success to Delete Items from the cart
	Given add the valid items table to cart
| itemId | quantity | price | amount    |
| 1      | 1        | 550   | 550       |
| 2      | 99       | 24.95 | 2470.05   |
| 3      | 999      | 22.5  | 22477.5   |
	When CARTADDITEM delete the items in table from cart
| itemId | quantity | price | amount    |
| 1      | 1        | 550   | 550       |
	Then should get  response of 'OK'
	And should get response comform with model 'CartItemDelete'
	And add  item:  ['datas'] ['cartId,accountNumber'] in response body to scenario context
	And should keep value constant for keys 'cartId,accountNumber'
	And CartInfo items in cart should be same to the table
| itemId | quantity | price | amount    |
| 2      | 99       | 24.95 | 2470.05   |
| 3      | 999      | 22.5  | 22477.5   |

Scenario: Should fail to delete items from cart with invalid credential
	Given add the valid items table to cart
| itemId | quantity | price | amount    |
| 1      | 1        | 550   | 550       |
	Given CARTADDITEM delete the valid items table from cart with invalid credential
| itemId | quantity | price | amount    |
| 1      | 1        | 550   | 550       |
Then should get  response of 'CredentialError'



Scenario: Should fail to delete items from empty cart
	Given CARTADDITEM delete the unexisting items  from cart 
| itemId	| quantity | price | amount    |
| 2			| 1        | 550   | 550       |
Then should get  response of 'InvalidItemError'

Scenario: Should fail to delete invalid items from cart
	Given add the valid items table to cart
| itemId | quantity | price | amount    |
| 1      | 1        | 550   | 550       |
And  CARTADDITEM delete the invalid items in table from cart
| itemId	| quantity | price | amount    |
| 0			| 1        | 550   | 550       |
Then should get  response of 'InvalidItemError'