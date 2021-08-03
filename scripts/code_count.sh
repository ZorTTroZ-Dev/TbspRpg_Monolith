#!/bin/bash

shopt -s globstar

declare -A arr
for f in ./**; do
  # if a directory, skip
  [[ -d "$f" ]] && continue
  lines=0
  # strip the extension
  ext="${f##*.}"
  # convert it to lowercase
  ext="${ext,,}"
  # if no dot in the name, extension is "empty"
  [[ ! $(basename "$f") =~ \. ]] && ext="empty"
  [[ $ext != "cs" ]] && continue
  # count the lines
  lines=$(wc -l "$f"| cut -d' ' -f1)
  # if lines equals to 0, skip
  [[ $lines -eq 0 ]] && continue
  # append the number of line to the array
  lines=$(( "${arr[$ext]}"+$lines ))
  arr[$ext]=$lines 
done

# loop over the array
for n in ${!arr[@]}; do
  echo "files $n: total lines ${arr[$n]}"
done

