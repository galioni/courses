from application.app.utils.timer import timer

timer.start()

def calculate(x):
    if x == 1:
        return 1
    else:
        return x * calculate(x-1)
      
print(calculate(50))

timer.end()