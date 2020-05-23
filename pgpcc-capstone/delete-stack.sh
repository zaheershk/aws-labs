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

aws ssm delete-parameter \
    --name /reportportal/bucket_name
	
echo " `date` : stack deleted successfully "
