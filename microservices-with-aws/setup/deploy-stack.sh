#!/usr/bin/env bash

# set global-variables
region_name="us-east-2"
stack_name="web-advert-stack"

echo " `date` : deploying stack "
aws cloudformation deploy \
	--template-file build.yml \
    --stack-name $stack_name \

echo " `date` : fetching output values "
# fetch outputs and store in local-variables
stack_info=$(aws cloudformation --region $region_name  describe-stacks --stack-name $stack_name)
cognito_userpool_id=$(aws cloudformation --region $region_name  describe-stacks --stack-name $stack_name --query "Stacks[0].Outputs[?OutputKey=='CognitoUserPool'].OutputValue" --output text)
cognito_userpool_appclient_id=$(aws cloudformation --region $region_name  describe-stacks --stack-name $stack_name --query "Stacks[0].Outputs[?OutputKey=='CognitoUserPoolAppClient'].OutputValue" --output text)
cognito_userpool_appclient_secret=$(aws cognito-idp describe-user-pool-client --client-id $cognito_userpool_appclient_id --user-pool-id $cognito_userpool_id --query "UserPoolClient.ClientSecret" --output text)

# echo $stack_info
# echo $cognito_userpool_id
# echo $cognito_userpool_appclient_id
# echo $cognito_userpool_appclient_secret

echo " `date` : setting configuration values into systems-manager parameter-store "
# store required key-values in systems-manager parameter-store
aws ssm put-parameter --name "/web-advert/AWS/UserPoolId" --value $cognito_userpool_id --type String
aws ssm put-parameter --name "/web-advert/AWS/UserPoolClientId" --value $cognito_userpool_appclient_id --type String
aws ssm put-parameter --name "/web-advert/AWS/UserPoolClientSecret" --value $cognito_userpool_appclient_secret --type SecureString

echo " `date` : stack deployed successfully "