#!/usr/bin/env bash

echo " `date` : part 1 - creating/updating parameters on Parameter Store "
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

aws ssm put-parameter \
	--name /reportportal/bucket_name \
	--type String \
	--value "capstone-s3-bucket-123" \
	--description "ReportPortal bucket name for binary storage" \
	--overwrite
  
echo " `date` : part 2 - deploying stack "
aws cloudformation deploy \
	--template-file reportportal-cfn.yml \
	--stack-name ReportPortalStack \
	--capabilities CAPABILITY_NAMED_IAM

echo " `date` : stack deployed successfully "
