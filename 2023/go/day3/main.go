package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"
	"unicode"
)

func main() {
	input, err := os.ReadFile("example3.txt")
	if err != nil {
		fmt.Println(err.Error())
	}
	schematic := string(input)

	// Part 1
	sum := sumPartNumbers(schematic)
	fmt.Println("Part 1:", sum)

	// Part 2
	sum = sumGearRatios(schematic)
	fmt.Println("Part 2:", sum)
}

func sumPartNumbers(schematic string) int {
	lines := strings.Split(schematic, "\n")
	sum := 0
	for x, line := range lines {
		for y := 0; y < len(line)-1; y++ {
			if unicode.IsDigit(rune(line[y])) {
				numEnd := y
				for numEnd <= len(line)-1 && unicode.IsDigit(rune(line[numEnd])) {
					numEnd++
				}
				number := line[y:numEnd]
				for i := range number {
					if hasAdjacentSymbol(lines, x, y+i) {
						n, _ := strconv.Atoi(number)
						sum += n
						break
					}
				}
				y = numEnd - 1
			}
		}
	}
	return sum
}

func hasAdjacentSymbol(lines []string, x, y int) bool {
	rows := len(lines) - 1
	cols := len(lines[0]) - 1
	if x > 0 && IsSymbol(lines[x-1][y]) {
		return true
	}
	if x < rows && IsSymbol(lines[x+1][y]) {
		return true
	}
	if y > 0 && IsSymbol(lines[x][y-1]) {
		return true
	}
	if y < cols && IsSymbol(lines[x][y+1]) {
		return true
	}
	if x > 0 && y > 0 && IsSymbol(lines[x-1][y-1]) {
		return true
	}
	if x < rows && y < cols && IsSymbol(lines[x+1][y+1]) {
		return true
	}
	if x > 0 && y < cols && IsSymbol(lines[x-1][y+1]) {
		return true
	}
	if x < rows && y > 0 && IsSymbol(lines[x+1][y-1]) {
		return true
	}
	return false
}

func IsSymbol(b byte) bool {
	r := rune(b)
	return !unicode.IsDigit(r) && !unicode.IsSpace(r) && r != '.'
}

func sumGearRatios(schematic string) int {
	lines := strings.Split(schematic, "\n")
	mp := make(map[[2]int]int)
	sum := 0
	for x, line := range lines {
		for y := 0; y < len(line)-1; y++ {
			if unicode.IsDigit(rune(line[y])) {
				numEnd := y
				for numEnd <= len(line)-1 && unicode.IsDigit(rune(line[numEnd])) {
					numEnd++
				}
				number := line[y:numEnd]
				for i := range number {
					if has, key := hasGear(lines, x, y+i); has {
						n, _ := strconv.Atoi(number)
						if gr, ok := mp[key]; ok {
							gr *= n
							sum += gr
							mp[key] = gr
						} else {
							mp[key] = n
						}
						break
					}
				}
				y = numEnd - 1
			}
		}
	}
	return sum
}

func hasGear(lines []string, x, y int) (bool, [2]int) {
	rows := len(lines) - 1
	cols := len(lines[0]) - 1
	if x > 0 && lines[x-1][y] == '*' {
		return true, [2]int{x - 1, y}
	}
	if x < rows && lines[x+1][y] == '*' {
		return true, [2]int{x + 1, y}
	}
	if y > 0 && lines[x][y-1] == '*' {
		return true, [2]int{x, y - 1}
	}
	if y < cols && lines[x][y+1] == '*' {
		return true, [2]int{x, y + 1}
	}
	if x > 0 && y > 0 && lines[x-1][y-1] == '*' {
		return true, [2]int{x - 1, y - 1}
	}
	if x < rows && y < cols && lines[x+1][y+1] == '*' {
		return true, [2]int{x + 1, y + 1}
	}
	if x > 0 && y < cols && lines[x-1][y+1] == '*' {
		return true, [2]int{x - 1, y + 1}
	}
	if x < rows && y > 0 && lines[x+1][y-1] == '*' {
		return true, [2]int{x + 1, y - 1}
	}
	return false, [2]int{-1, -1}
}
