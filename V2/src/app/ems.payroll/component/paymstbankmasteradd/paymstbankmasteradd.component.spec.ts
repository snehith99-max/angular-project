import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymstbankmasteraddComponent } from './paymstbankmasteradd.component';

describe('PaymstbankmasteraddComponent', () => {
  let component: PaymstbankmasteraddComponent;
  let fixture: ComponentFixture<PaymstbankmasteraddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PaymstbankmasteraddComponent]
    });
    fixture = TestBed.createComponent(PaymstbankmasteraddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
