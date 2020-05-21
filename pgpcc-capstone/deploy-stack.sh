#!/usr/bin/env bash

echo " `date` : part 1 - creating AWS EC2 key-pair "
aws ec2 create-key-pair \
	--key-name ReportPortal

echo " `date` : part 2 - creating/updating parameters on Parameter Store "
aws ssm put-parameter \
	--name /reportportal/db_name \
	--type String \
	--value "reportportal" \
	--description "ReportPortal database name" \
	--overwrite

aws ssm put-parameter \
	--name /reportportal/master_username \
	--type String \
	--value "rpuser" \
	--description "ReportPortal master-username for DB" \
	--overwrite

aws ssm put-parameter \
	--name /reportportal/master_password \
	--type SecureString \
	--value "5up3r53cr3tPa55w0rd" \
	--description "ReportPortal master-password for DB" \
	--overwrite
  
echo " `date` : part 3 - deploying stack "
aws cloudformation deploy \
	--template-file reportportal-cfn.yml \
	--stack-name ReportPortalStack

echo " `date` : stack deployed successfully "
