Feature: SpecFlowTryOutStudy
	In order to find the answer 
	As a tester idiot
	I want to roll on my ways to solutions from here

@mytag
Scenario: try the ExecuteAsync  in Example CartViewInfo
	Given CartInfo Register And Login And CreateCart
	When visit the cart info API with valid credential
	Then CartInfo should give  response of 'OK'
