Feature: SpecFlowStudy
	In order to make transaction order
	As a user
	I want to get the transaction number
@mytag
Scenario: C501 Should fail to delete items from empty cart
	Given CARTADDITEM delete the unexisting items  from cart 
| itemId	| quantity | price | amount    |
| 2			| 1        | 550   | 550       |
Then should get  response of 'InvalidItemError'
