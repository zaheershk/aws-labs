#!/usr/bin/env bash

echo " `date` : part 1 - deleting stack "
aws cloudformation delete-stack \
	--stack-name ReportPortalStack

echo " `date` : part 2 - deleting parameters from Parameter Store "
aws ssm delete-parameter \
    --name /reportportal/db_name
	
aws ssm delete-parameter \
    --name /reportportal/master_username
	
aws ssm delete-parameter \
    --name /reportportal/master_password
	
echo " `date` : part 3 - deleting AWS EC2 key-pair "
aws ec2 delete-key-pair --key-name ReportPortal

echo " `date` : stack deleted successfully "
