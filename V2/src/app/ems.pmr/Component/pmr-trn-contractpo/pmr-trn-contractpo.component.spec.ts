import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnContractpoComponent } from './pmr-trn-contractpo.component';

describe('PmrTrnContractpoComponent', () => {
  let component: PmrTrnContractpoComponent;
  let fixture: ComponentFixture<PmrTrnContractpoComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnContractpoComponent]
    });
    fixture = TestBed.createComponent(PmrTrnContractpoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
