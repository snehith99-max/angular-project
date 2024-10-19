import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnContractvendorComponent } from './pmr-trn-contractvendor.component';

describe('PmrTrnContractvendorComponent', () => {
  let component: PmrTrnContractvendorComponent;
  let fixture: ComponentFixture<PmrTrnContractvendorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnContractvendorComponent]
    });
    fixture = TestBed.createComponent(PmrTrnContractvendorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
