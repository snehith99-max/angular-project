import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnStockamendComponent } from './ims-trn-stockamend.component';

describe('ImsTrnStockamendComponent', () => {
  let component: ImsTrnStockamendComponent;
  let fixture: ComponentFixture<ImsTrnStockamendComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnStockamendComponent]
    });
    fixture = TestBed.createComponent(ImsTrnStockamendComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
