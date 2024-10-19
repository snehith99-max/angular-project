import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SbcMstDynamicdbcreationComponent } from './sbc-mst-dynamicdbcreation.component';

describe('SbcMstDynamicdbcreationComponent', () => {
  let component: SbcMstDynamicdbcreationComponent;
  let fixture: ComponentFixture<SbcMstDynamicdbcreationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SbcMstDynamicdbcreationComponent]
    });
    fixture = TestBed.createComponent(SbcMstDynamicdbcreationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
