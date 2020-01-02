Feature: Authentication
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@Authentication @Scenario_1 @RegisterUser
Scenario: Verify Post operation for registering a new account with new username and password
	Given I perform a RegisterUser POST operation for e-store/authentication/register with body
		| key      | value    |
		| username | username |
		| password | password |
	Then I should have the response with status code OK
	And I should save the accountNumber from the response

@Authentication @Scenario_2 @RegisterUser
Scenario: Verify Post operation for registering a new account with existing username and password
	Given I register a new account with new username and password
		| key      | value    |
		| username | username |
		| password | password |
	When I register a new account with above username and password
	Then error message should say Error：Registered account！ 
	
@Authentication @Scenario_3 @RegisterUser
Scenario: Register with previous username once the related account was deleted
	Given I register a new account with new username and password
		| key      | value    |
		| username | username |
		| password | password |
	When I delete the above user account
	And I register a new account with above username and password
	Then I should have the response with status code OK

@Authentication @Scenario_4 @RegisterUser
Scenario: Fail to get item information without invalid auth
	Given I register a new account with new username and password
		| key      | value    |
		| username | username |
		| password | password |
	And I get JWT authentication of user with following credentials
		| key       | value    |
		| username  | username |
		| password  | password |
		| browserId | abcdef   |
	And I perform a GET operation for e-store/sales-data/items
	And I perform GET operation for SalesData with invalid auth
	Then error message should say Error：你还没有登录！


@Authentication @Scenario_5 @RegisterUser
Scenario: Able to get item information without valid auth
	Given I register a new account with new username and password
		| key      | value    |
		| username | username |
		| password | password |
	And I get JWT authentication of user with following credentials
		| key       | value    |
		| username  | username |
		| password  | password |
		| browserId | abcdef   |
	And I perform a GET operation for e-store/sales-data/items
	And I perform operation for GET SalesData
	Then I should have the response with status code OK


@Authentication
Scenario: Verify Post operation for successful authentication of an existing customer
	Given I get JWT authentication of user with following credentials
		| key       | value      |
		| username  | apitest010 |
		| password  | apitest    |
		| browserId | abcdef     |
	Then I should have the response with status code OK
	#And bearer token that expires in 7200