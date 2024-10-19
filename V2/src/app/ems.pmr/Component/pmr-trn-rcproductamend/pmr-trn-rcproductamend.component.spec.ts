import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnRCproductamendComponent } from './pmr-trn-rcproductamend.component';

describe('PmrTrnRCproductamendComponent', () => {
  let component: PmrTrnRCproductamendComponent;
  let fixture: ComponentFixture<PmrTrnRCproductamendComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnRCproductamendComponent]
    });
    fixture = TestBed.createComponent(PmrTrnRCproductamendComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
