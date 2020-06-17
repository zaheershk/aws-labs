#!/usr/bin/env bash

echo " `date` : deleting configuration values from systems-manager parameter-store "
aws ssm delete-parameters \
    --names "/web-advert/AWS/UserPoolId" "/web-advert/AWS/UserPoolClientId" "/web-advert/AWS/UserPoolClientSecret"

echo " `date` : deleting emptying and deleting S3 bucket "
aws s3 rm s3://sz-webadvert-images --recursive
aws s3 rb s3://sz-webadvert-images --force

echo " `date` : deleting stack "
aws cloudformation delete-stack \
	--stack-name web-advert-stack
	
echo " `date` : stack deleted successfully "