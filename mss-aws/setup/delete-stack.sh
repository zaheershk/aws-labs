#!/usr/bin/env bash

echo " `date` : deleting configuration values from systems-manager parameter-store "
aws ssm delete-parameters \
    --names "/web-advert/AWS/UserPoolId" "/web-advert/AWS/UserPoolClientId" "/web-advert/AWS/UserPoolClientSecret"

echo " `date` : deleting stack "
aws cloudformation delete-stack \
	--stack-name web-advert-stack
	
echo " `date` : stack deleted successfully "