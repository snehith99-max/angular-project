import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SbcMstProductmoduleComponent } from './sbc-mst-productmodule.component';

describe('SbcMstProductmoduleComponent', () => {
  let component: SbcMstProductmoduleComponent;
  let fixture: ComponentFixture<SbcMstProductmoduleComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SbcMstProductmoduleComponent]
    });
    fixture = TestBed.createComponent(SbcMstProductmoduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
