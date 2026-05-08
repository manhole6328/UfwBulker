An C# console app for converting IPs in json into ufw rules.
#### Usage:
```
ufwbulker <app_name> <input_file> [output_dir]
```


#### Input:
```
# addreses.json
[
    "subnet": "95.104.246.0/24",
    "comment": "MTS ASN"
  },
  {
    "subnet": "192.162.0.0/22",
    "comment": "MTS ASN"
  },
  {
    "subnet": "81.195.180.0/22",
    "comment": "MTS ASN"
  },
  {
    "subnet": "212.30.184.0/23",
    "comment": "MTS ASN"
  },
  {
    "subnet": "85.140.92.4/32",
    "comment": "MTS ASN"
  }
]
```


#### Output :
```
# add-script.bash
#!/bin/bash
set -e
ufw allow from 95.104.246.0/24 to any app app comment 'MTS ASN'
ufw allow from 192.162.0.0/22 to any app app comment 'MTS ASN'
ufw allow from 81.195.180.0/22 to any app app comment 'MTS ASN'
ufw allow from 212.30.184.0/23 to any app app comment 'MTS ASN'
ufw allow from 85.140.92.4/32 to any app app comment 'MTS ASN'
```

#### IPv6 also works!