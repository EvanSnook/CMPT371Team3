fp = open("tmp/vsoutput.txt")
coverage = ".coverage"
for line in fp:
 if line.find(coverage) > -1:
  os.rename(line.strip(), "tmp/raw.coverage")


fp.close()